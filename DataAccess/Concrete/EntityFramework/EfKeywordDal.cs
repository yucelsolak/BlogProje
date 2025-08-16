using Core.DataAccess.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.Blog;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfKeywordDal : EfEntityRepositoryBase<Keyword, BlogContext>, IKeywordDal
    {
        public int DeleteIfUnusedByIds(IEnumerable<int> ids)
        {
            var list = ids?.Distinct().ToList();
            if (list == null || list.Count == 0) return 0;

            using var ctx = new BlogContext();
            var idCsv = string.Join(",", list);
            // Yalnızca HIÇBIR blogda kullanılmayanları sil
            return ctx.Database.ExecuteSqlRaw($@"
        DELETE k
        FROM Keywords k
        LEFT JOIN KeywordBlogs kb ON k.KeywordId = kb.KeywordId
        WHERE kb.KeywordId IS NULL
          AND k.KeywordId IN ({idCsv});
    ");
        }

        public List<int> GetKeywordIdsByBlogId(int blogId)
        {
            using var ctx = new BlogContext();
            return ctx.Set<KeywordBlog>()
                      .Where(kb => kb.BlogId == blogId)
                      .Select(kb => kb.KeywordId)
                      .Distinct()
                      .ToList();
        }

        public IList<KeywordLinkDto> GetLinksForBlog(int blogId)
        {
            using var ctx = new BlogContext();
            return (from k in ctx.Set<Keyword>()
                    join kb in ctx.Set<KeywordBlog>() on k.KeywordId equals kb.KeywordId
                    join s in ctx.Set<Slug>() on k.KeywordId equals s.EntityId
                    where kb.BlogId == blogId && s.EntityType == "Keyword"
                    orderby k.KeywordName
                    select new KeywordLinkDto { KeywordName = k.KeywordName, Slug = s.SlugText }).ToList();
        }

        public IList<string> GetNamesByBlogId(int blogId)
        {
            using var ctx = new BlogContext();
            return (from k in ctx.Set<Keyword>()
                    join kb in ctx.Set<KeywordBlog>() on k.KeywordId equals kb.KeywordId
                    where kb.BlogId == blogId
                    orderby k.KeywordName
                    select k.KeywordName).ToList();
        }

        public bool IsKeywordUsed(int keywordId)
        {
            using var ctx = new BlogContext();
            return ctx.Set<KeywordBlog>()
                      .Any(kb => kb.KeywordId == keywordId);
        }

        public IList<string> SuggestNames(string term, int limit = 10)
        {
            term = (term ?? "").Trim();
            if (term.Length == 0) return new List<string>();

            using var ctx = new BlogContext();
            return ctx.Set<Keyword>()
                      .Where(k => k.KeywordName.StartsWith(term)) // SQL Server çoğu kurulumda CI -> büyük/küçük duyarsız
                      .OrderBy(k => k.KeywordName)
                      .Select(k => k.KeywordName)
                      .Take(limit)
                      .ToList();
        }
    }
}
