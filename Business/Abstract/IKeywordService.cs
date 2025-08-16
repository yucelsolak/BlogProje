using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IKeywordService:IGenericService<Keyword>
    {
        List<int> UpsertAndGetIds(IEnumerable<string> names);
        IList<string> GetNamesByBlogId(int blogId);
        string GetCsvByBlogId(int blogId); // UI için pratik
        IList<string> Suggest(string term, int limit = 10);
        IList<KeywordLinkDto> GetLinksForBlog(int blogId);
        IResult UpdateKeyword (KeywordLinkDto dto);
        List<KeywordLinkDto> SearchKeyword(string searchTerm);
    }
}
