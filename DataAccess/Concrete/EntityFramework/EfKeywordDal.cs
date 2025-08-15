using Core.DataAccess.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
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
        public void DeleteRangeByIds(IEnumerable<int> ids)
        {
            var list = ids?.Distinct().ToList();
            if (list == null || list.Count == 0) return;

            using var ctx = new BlogContext();
            var toDelete = ctx.Set<Keyword>().Where(k => list.Contains(k.KeywordId)).ToList();
            ctx.RemoveRange(toDelete);
            ctx.SaveChanges();
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

        public bool IsKeywordUsed(int keywordId)
        {
            using var ctx = new BlogContext();
            return ctx.Set<KeywordBlog>()
                      .Any(kb => kb.KeywordId == keywordId);
        }
    }
}
