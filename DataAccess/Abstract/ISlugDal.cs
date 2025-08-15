using Core.DataAccess;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface ISlugDal:IEntityRepository<Slug>
    {
        void DeleteByEntity(string entityType, int entityId);
        void DeleteByEntities(string entityType, IEnumerable<int> entityIds);
    }
}
