using Core.DataAccess.Concrete;
using Core.Extensions;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfBlogDal : EfEntityRepositoryBase<Blog, BlogContext>, IBlogDal
    {
        public List<Blog> GetAllByCategory(int CategoryId)
        {
            using (var context = new BlogContext())
            {
                return context.Blogs
                    .Where(b => b.CategoryId == CategoryId)
                    .ToList();
            }
        }

        public List<BlogListDto> GetLastTenBlog()
        {
            using (var context= new BlogContext())
            {
                var raw = context.Blogs
           .AsNoTracking()
           .OrderByDescending(b => b.BlogId)
           .Select(b => new
           {
               b.BlogId,
               b.Title,
               b.Image,
               b.AddedTime,
               b.Description,
               b.Slug
           })
           .Take(10)
           .ToList(); // <-- DB çağrısı burada biter

                // Kısaltmayı bellekte uygula (extension metodları burada devreye girer)
                return raw.Select(b => new BlogListDto
                {
                    BlogId = b.BlogId,
                    Title = b.Title,
                    Image = b.Image,
                    AddedTime = b.AddedTime,
                    Slug=b.Slug,// DateTimeOffset
                    ShortDescription = b.Description.ToShort(160) // veya .ToWordShort(40)
                })
                .ToList();
            } 
        }

        public List<BlogListDto> GetByCategory(int categoryId)
        {
            using var context = new BlogContext();
            return context.Blogs
                .AsNoTracking()
                .Where(b => b.Status && b.CategoryId == categoryId)
                .OrderByDescending(b => b.CategoryId)
                .Select(b => new { b.BlogId, b.Title, b.Image, b.AddedTime, b.Description,b.Slug })
                .ToList()
                .Select(b => new BlogListDto
                {
                    BlogId = b.BlogId,
                    Title = b.Title,
                    Image = b.Image,
                    AddedTime = b.AddedTime,
                    BlogSlug=b.Slug,
                    ShortDescription = b.Description.ToShort(200) // veya .ToWordShort(40)
                })
                .ToList();
        }

        public List<BlogListDto> GetAllBlog()
        {
            using var context = new BlogContext();
            return context.Blogs
                .AsNoTracking()
                .Where(b => b.Status)
                .OrderByDescending(b => b.BlogId)
                .Select(b => new { b.BlogId, b.Title, b.Image, b.AddedTime, b.Description,b.Slug,b.ViewCount,b.Category.CategoryName })
                .ToList()
                .Select(b => new BlogListDto
                {
                    BlogId = b.BlogId,
                    Title = b.Title,
                    Image = b.Image,
                    BlogSlug = b.Slug,
                    AddedTime = b.AddedTime, 
                    ViewCount=b.ViewCount,
                    CategoryName=b.CategoryName,
                    ShortDescription = b.Description.ToShort(200) // veya .ToWordShort(40)
                })
                .ToList();

        }

        public Blog GetBlogDetail(string slug)
        {
            using var context = new BlogContext();
            return context.Blogs
        .AsNoTracking()
        .FirstOrDefault(b => b.Slug == slug && b.Status);
        }

        public void IncrementViewCount(int BlogId)
        {
            using var context= new BlogContext();
            context.Database.ExecuteSqlRaw(
                "UPDATE [dbo].[Blogs] SET [ViewCount] = [ViewCount] + 1 WHERE [BlogId] = {0}", BlogId);
        }

        public List<BlogListDto> GetMostRead()
        {
            using var context = new BlogContext();
                return context.Blogs
                .AsNoTracking()
                .Where(b => b.Status)
                .OrderByDescending(b => b.ViewCount)
                .Select(b => new { b.BlogId, b.Title, b.Image, b.AddedTime, b.Description,b.Slug,b.ViewCount })
                .Take(10)
                .ToList()
            .Select(b => new BlogListDto
             {
                 BlogId = b.BlogId,
                 Title = b.Title,
                 Image = b.Image,
                 BlogSlug = b.Slug,
                 AddedTime = b.AddedTime,
                 ViewCount = b.ViewCount,
                 ShortDescription = b.Description.ToShort(200) // veya .ToWordShort(40)
             })
                .ToList();
        }

        public List<BlogListDto> GetAdmin50Blog()
        {
            using var context=new BlogContext();
            return context.Blogs
                .AsNoTracking()
                .Where(b => b.Status)
                .OrderByDescending (b => b.BlogId)
                .Select(b=>new { b.BlogId, b.Title,b.Image, b.AddedTime,b.Status,b.Slug,b.ViewCount,b.Category.CategoryName,b.CategoryId })
                .Take (50)
                .ToList()
                .Select(b=> new BlogListDto
                {
                    BlogId = b.BlogId,
                    Title = b.Title,
                    Image = b.Image,
                    BlogSlug = b.Slug,
                    AddedTime = b.AddedTime,
                    ViewCount = b.ViewCount,
                    CategoryName=b.CategoryName,
                    CategoryId=b.CategoryId,
                    Status=b.Status,
                }).ToList();
        }
    }
}
