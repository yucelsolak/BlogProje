using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules;
using Core.Aspects.Autofac.Validation;
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
        [ValidationAspect(typeof(CategoryValidator))]
        public IResult AddCategory(UpdateCategoryDto dto)
        {
            var existingCategory = _blogCategoryDal.Get(c => c.CategoryName.ToLower() == dto.CategoryName.ToLower());
            if (existingCategory != null)
                return new ErrorResult(Messages.CategorySameName);

            var entity = _mapper.Map<BlogCategory>(dto);
            _blogCategoryDal.Add(entity);

            _slugService.AddSlug(dto.CategoryName, "Category", entity.CategoryId);
            return new SuccessResult(Messages.CategoryAdded);
        }
        [ValidationAspect(typeof(CategoryValidator))]
        public IResult CategoryWithSlugUpdate(UpdateCategoryDto entity)
        {
            var normalizedName = entity.CategoryName.Trim();

            var exists = _blogCategoryDal
        .GetAll(c => c.CategoryId != entity.CategoryId &&
                      c.CategoryName.ToLower() == normalizedName.ToLower())
        .Any(); // <- Burada Any, List/Enumerable üstünde

            if (exists)
                return new ErrorResult(Messages.CategorySameName);

            var category = new BlogCategory
            {
                CategoryId = entity.CategoryId,
                CategoryName = entity.CategoryName,
                Status = entity.Status
            };

            _blogCategoryDal.Update(category);

            var slug = _slugService.GetByEntity("Category", entity.CategoryId);
            if (slug != null)
            {
                _slugService.UpdateSlug(slug.SlugId, entity.CategoryName);
            }
            return new SuccessResult(Messages.CategoryUpdated);
        }

        public BlogCategory GetBySlug(string slug)
        {
           return _blogCategoryDal.GetBySlug(slug);
        }

        public List<CategoryWithBlogCountDto> GetCategoriesWithBlogCount()
        {
            return _blogCategoryDal.GetCategoriesWithBlogCount();
        }

        public IResult TAdd(BlogCategory entity)
        {
            _blogCategoryDal.Add(entity);
            return new SuccessResult();
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
