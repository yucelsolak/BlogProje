using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IBlogService:IGenericService<Blog>
    {
        List<Blog> GetAllByCategory(int CategoryId);
        List<Blog> GetLastTenBlogs();
        List<BlogListDto> GetBlogListDtos();
    }
}
