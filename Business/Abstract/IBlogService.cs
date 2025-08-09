using Entities.Concrete;
using Entities.DTOs.Blog;
using Entities.DTOs.Category;
using Microsoft.AspNetCore.Http;
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
        
        void AddBlog(AddUpdateBlogDto dto);
        Task<string> SaveBlogImage(IFormFile blogImage, string title);
        Task BlogWithSlugUpdate(Blog entity, string slugText, IFormFile newImage);
        Task UpdateBlogImages(int blogId, IFormFile newImage, string slug);

        List<BlogListDto> SearchBlogAdmin(string searchTerm);
    }
}
