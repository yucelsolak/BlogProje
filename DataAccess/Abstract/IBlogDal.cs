using Core.DataAccess;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IBlogDal:IEntityRepository<Blog>
    {
        
        List<BlogListDto> GetLastTenBlog();
        List<BlogListDto> GetByCategory(int CategoryId);
        List<BlogListDto> GetAllBlog();
        List<BlogListDto> GetAdmin50Blog();
        List<BlogListDto> GetMostRead();
        Blog GetBlogDetail(string Slug);
        void IncrementViewCount(int BlogId);
    }
}
