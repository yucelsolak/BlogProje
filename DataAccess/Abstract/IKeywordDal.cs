using Core.DataAccess;
using Entities.Concrete;
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
        void DeleteRangeByIds(IEnumerable<int> ids);
    }
}
