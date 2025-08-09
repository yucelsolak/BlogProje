using AutoMapper;
using Business.Abstract;
using Core.Entities;
using Core.Extensions;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class BlogCategoryManager : IBlogCategoryService
    {
        IBlogCategoryDal _blogCategoryDal;
        IMapper _mapper;
        ISlugDal _slugDal;
        ISlugService _slugService;
        IBlogDal _blogDal;
        public BlogCategoryManager(IBlogCategoryDal blogCategoryDal, IMapper mapper, ISlugDal slugDal, ISlugService slugService, IBlogDal blogDal)
        {
            _blogCategoryDal = blogCategoryDal;
            _mapper = mapper;
            _slugDal = slugDal;
            _slugService = slugService;
            _blogDal = blogDal;
        }

        public void AddCategory(AddCategoryDto dto)
        {
            var entity = _mapper.Map<BlogCategory>(dto);
            _blogCategoryDal.Add(entity); 

            var slugText = dto.CategoryName.ToSlug();

            var slug = new Slug
            {
                SlugText = slugText,
                EntityType = "Category",
                EntityId = entity.CategoryId 
            };

            _slugDal.Add(slug); 
        }

        public bool CanDeleteCategory(int categoryId)
        {
            var blogs = _blogDal.GetByCategory(categoryId);
            return blogs == null || !blogs.Any(); // Eğer bağlı blog yoksa true döner, yoksa false
        }

        public void CategoryWithSlugUpdate(BlogCategory entity, string slugText)
        {
            
            _blogCategoryDal.Update(entity);

            var slug = _slugService.GetByEntity("Category", entity.CategoryId);
            if (slug != null)
            {
                slug.SlugText = slugText.ToSlug();
                _slugDal.Update(slug);
            }
        }

        public BlogCategory GetBySlug(string slug)
        {
           return _blogCategoryDal.GetBySlug(slug);
        }

        public List<CategoryWithBlogCountDto> GetCategoriesWithBlogCount()
        {
            return _blogCategoryDal.GetCategoriesWithBlogCount();
        }

        public void TAdd(BlogCategory entity)
        {
            _blogCategoryDal.Add(entity);
        }

        public void TDelete(BlogCategory entity)
        {
            if (!CanDeleteCategory(entity.CategoryId))
            {
                throw new InvalidOperationException("Bu kategoride bloglar mevcut. Bu nedenle silinemez.");
            }

            _slugService.DeleteByEntity("Category", entity.CategoryId);
            _blogCategoryDal.Delete(entity);
            
        }

        public BlogCategory TGetByID(int id)
        {
            return _blogCategoryDal.Get(p=>p.CategoryId == id);
        }

        public List<BlogCategory> TGetList()
        {
            return _blogCategoryDal.GetAll();
        }

        public void TUpdate(BlogCategory entity)
        {
            throw new NotImplementedException();
        }
    }
}
