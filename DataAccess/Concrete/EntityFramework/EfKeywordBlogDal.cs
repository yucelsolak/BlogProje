using Core.DataAccess.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfKeywordBlogDal : EfEntityRepositoryBase<KeywordBlog, BlogContext>, IKeywordBlogDal
    {
        public List<int> GetKeywordIdsByBlogId(int blogId)
        {
            using var ctx = new BlogContext();
            return ctx.Set<KeywordBlog>()
                      .Where(kb => kb.BlogId == blogId)
                      .Select(kb => kb.KeywordId)
                      .Distinct()
                      .ToList();
        }
    }
}
