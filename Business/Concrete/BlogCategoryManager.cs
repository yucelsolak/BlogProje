using AutoMapper;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
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


        public BlogCategoryManager(IBlogCategoryDal blogCategoryDal)
        {
           _blogCategoryDal = blogCategoryDal; 
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
            _blogCategoryDal.Update(entity);
        }
    }
}
