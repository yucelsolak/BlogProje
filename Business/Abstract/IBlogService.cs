using Core.Utilities.Results;
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

        IResult AddBlog(AddUpdateBlogDto dto);
        Task<string> SaveBlogImage(IFormFile blogImage, string title);
        Task<IResult> BlogWithSlugUpdate(AddUpdateBlogDto dto);
        Task UpdateBlogImages(int blogId, IFormFile newImage, string slug);
        List<Blog> GetByKeywordId(int keywordId);
        List<BlogListDto> SearchBlogAdmin(string searchTerm);
        List<BlogListDto> GetBlogListByKeywordId(int keywordId);
    }
}
