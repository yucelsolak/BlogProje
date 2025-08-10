using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class SlugService:ISlugService
    {
        ISlugDal _slugDal;

        public SlugService(ISlugDal slugDal)
        {
            _slugDal = slugDal;   
        }

        public void DeleteByEntity(string entityType, int entityId)
        {
            var slug = _slugDal.Get(x => x.EntityType == entityType && x.EntityId == entityId);
            if (slug != null)
            {
                _slugDal.Delete(slug);
            }
        }

        public Slug GetByEntity(string entityType, int entityId)
        {
            return _slugDal.Get(x => x.EntityType == entityType && x.EntityId == entityId);
        }

        public async Task<string> GetSlugAsync(string entityType, int entityId)
        {
            using var context = new BlogContext();

            var slug = await context.Slugs
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.EntityType == entityType && s.EntityId == entityId);

            return slug?.SlugText ?? "";
        }

        public async Task<Dictionary<int, string>> GetSlugsByEntityTypeAsync(string entityType)
        {
            using var context = new BlogContext();

            return await context.Slugs
                .AsNoTracking()
                .Where(s => s.EntityType == entityType)
                .ToDictionaryAsync(s => s.EntityId, s => s.SlugText);
        }

        public void TAdd(Slug entity)
        {
            _slugDal.Add(entity);
        }

        public IResult TDelete(Slug entity)
        {
            throw new NotImplementedException();
        }

        public Slug TGetByID(int id)
        {
            throw new NotImplementedException();
        }

        public List<Slug> TGetList()
        {
            throw new NotImplementedException();
        }

        public void TUpdate(Slug entity)
        {
            throw new NotImplementedException();
        }
    }
}
