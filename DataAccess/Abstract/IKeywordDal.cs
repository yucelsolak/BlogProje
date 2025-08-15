using Core.DataAccess;
using Entities.Concrete;
using Entities.DTOs.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IKeywordDal:IEntityRepository<Keyword>
    {
        List<int> GetKeywordIdsByBlogId(int blogId);
        bool IsKeywordUsed(int keywordId);
        IList<string> GetNamesByBlogId(int blogId);
        int DeleteIfUnusedByIds(IEnumerable<int> ids);
        IList<string> SuggestNames(string term, int limit = 10);
        IList<KeywordLinkDto> GetLinksForBlog(int blogId);
    }
}
