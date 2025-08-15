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
    public class EfSlugDal : EfEntityRepositoryBase<Slug, BlogContext>, ISlugDal
    {
        public int DeleteKeywordSlugsIfNoKeywordByIds(IEnumerable<int> ids)
        {
            var list = ids?.Distinct().ToList();
            if (list == null || list.Count == 0) return 0;

            using var ctx = new BlogContext();
            var idCsv = string.Join(",", list);
            // Sadece Keywords'te karşılığı kalmamış olanların sluglarını sil
            return ctx.Database.ExecuteSqlRaw($@"
        DELETE s
        FROM Slugs s
        WHERE s.EntityType = 'Keyword'
          AND s.EntityId IN ({idCsv})
          AND NOT EXISTS (SELECT 1 FROM Keywords k WHERE k.KeywordId = s.EntityId);
    ");
        }
    }
}
