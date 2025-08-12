using Business.Abstract;
using Core.Extensions;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Business.Concrete
{
    public class SlugManager:ISlugService
    {
        ISlugDal _slugDal;

        public SlugManager(ISlugDal slugDal)
        {
            _slugDal = slugDal;   
        }

        public Slug AddSlug(string slugText, string entityType, int entityId)
        {
            var newslugText = GenerateUniqueSlug(slugText);

            var slug = new Slug
            {
                SlugText = newslugText,
                EntityType = entityType,
                EntityId = entityId
            };

            _slugDal.Add(slug);
            return slug;
        }

        public void DeleteByEntity(string entityType, int entityId)
        {
            var slug = _slugDal.Get(x => x.EntityType == entityType && x.EntityId == entityId);
            if (slug != null)
            {
                _slugDal.Delete(slug);
            }
        }

        private string GenerateUniqueSlug(string baseText, int? ignoreSlugId = null)
        {
            var slugText = baseText.ToSlug();
            var uniqueSlug = slugText;
            int counter = 1;

            while (_slugDal.GetAll(s =>
                   s.SlugText == uniqueSlug && (!ignoreSlugId.HasValue || s.SlugId != ignoreSlugId.Value)).Any())
            {
                uniqueSlug = $"{slugText}{counter}";
                counter++;
            }

            return uniqueSlug;
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

        public IResult TAdd(Slug entity)
        {
            _slugDal.Add(entity);
            return new SuccessResult();
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

        public Slug UpdateSlug(int slugId, string newText)
        {
            var slug = _slugDal.Get(s => s.SlugId == slugId);
            if (slug == null) throw new Exception("Slug bulunamadı");

            var slugText = GenerateUniqueSlug(newText, slugId); // kendi kaydını hariç tut

            slug.SlugText = slugText;
            _slugDal.Update(slug);

            return slug;
        }
    }
}
