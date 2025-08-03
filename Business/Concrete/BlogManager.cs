using AutoMapper;
using Business.Abstract;
using Core.Extensions;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class BlogManager : IBlogService
    {
        IBlogDal _blogDal;


        public BlogManager(IBlogDal blogDal)
        {
            _blogDal = blogDal;
        }

        public List<BlogListDto> GetByCategory(int CategoryId)
        {
            return _blogDal.GetByCategory(CategoryId);
        }

         public List<BlogListDto> GetLastTenBlog()
        {
            return _blogDal.GetLastTenBlog();
        }

        public void TAdd(Blog entity)
        {
            _blogDal.Add(entity);
        }

        public void TDelete(Blog entity)
        {
            _blogDal.Delete(entity);
        }

        public Blog TGetByID(int id)
        {
           return _blogDal.Get(p => p.BlogId == id);
        }

        public List<Blog> GetAllBlog()
        {
            return _blogDal.GetAll();
        }

        public void TUpdate(Blog entity)
        {
            _blogDal.Update(entity);
        }

        List<BlogListDto> IBlogService.GetAllBlog()
        {
           return _blogDal.GetAllBlog();
        }

        public List<Blog> TGetList()
        {
            return _blogDal.GetAll();
        }

        public Blog GetBlogDetail(string slug)
        {
            return _blogDal.GetBlogDetail(slug);
        }

        public void IncrementViewCount(int BlogId)
        {
            _blogDal.IncrementViewCount(BlogId);
        }

        public List<BlogListDto> GetMostRead()
        {
            return _blogDal.GetMostRead();
        }

        public List<BlogListDto> GetAdmin50Blog()
        {
            return _blogDal.GetAdmin50Blog();
        }
    }
}
