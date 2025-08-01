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
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class BlogManager : IBlogService
    {
        IBlogDal _blogDal;
        IMapper _mapper;


        public BlogManager(IBlogDal blogDal, IMapper mapper)
        {
            _blogDal = blogDal;
            _mapper = mapper;
        }
        public List<Blog> GetAllByCategory(int CategoryId)
        {
            return _blogDal.GetAll(p => p.CategoryId == CategoryId);
        }

        public List<BlogListDto> GetBlogListDtos()
        {
            var blogs = _blogDal.GetAll().OrderByDescending(b=>b.BlogId).Take(10);
            return _mapper.Map<List<BlogListDto>>(blogs);

        }

        public List<Blog> GetLastTenBlogs()
        {
            return _blogDal.GetAll().OrderByDescending(p=>p.BlogId).Take(10).ToList();
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

        public List<Blog> TGetList()
        {
            return _blogDal.GetAll();
        }

        public void TUpdate(Blog entity)
        {
            _blogDal.Update(entity);
        }
    }
}
