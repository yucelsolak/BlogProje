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
    public class EfSlugDal : EfEntityRepositoryBase<Slug, BlogContext>, ISlugDal
    {
        public void DeleteByEntities(string entityType, IEnumerable<int> entityIds)
        {
            throw new NotImplementedException();
        }

        public void DeleteByEntity(string entityType, int entityId)
        {
            throw new NotImplementedException();
        }
    }
}
