using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Core.Entities;
using Core.Extensions;
using Core.Utilities.Results;
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

        public IResult AddCategory(AddCategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto?.CategoryName))
                return new ErrorResult(Messages.CategoryNotAllowEmpty);

            var existingCategory = _blogCategoryDal.Get(c => c.CategoryName.ToLower() == dto.CategoryName.ToLower());
            if (existingCategory != null)
                return new ErrorResult(Messages.CategorySameName);

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
            return new SuccessResult(Messages.CategoryAdded);
        }

        //public bool CanDeleteCategory(int categoryId)
        //{
        //    var blogs = _blogDal.GetByCategory(categoryId);
        //    return blogs == null || !blogs.Any(); // Eğer bağlı blog yoksa true döner, yoksa false
        //}

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

        public IResult TDelete(BlogCategory entity)
        {
            var hasBlogs = _blogDal.GetByCategory(entity.CategoryId)?.Any() == true;
            if (hasBlogs)
                return new ErrorResult(Messages.CategoryHasBlogs);

            _blogCategoryDal.Delete(entity);
            _slugService.DeleteByEntity("Category", entity.CategoryId);
            return new SuccessResult(Messages.CategoryDeleted);
  
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
