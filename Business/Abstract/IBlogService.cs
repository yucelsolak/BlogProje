using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IBlogService:IGenericService<Blog>
    {
        List<BlogListDto> GetByCategory(int CategoryId);
        List<BlogListDto> GetLastTenBlog();
        List<BlogListDto> GetAllBlog();
        void IncrementViewCount(int BlogId);
        Blog GetBlogDetail(string slug);
        List<BlogListDto> GetMostRead();
        List<BlogListDto> GetAdmin50Blog();

    }
}
