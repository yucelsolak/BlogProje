using Core.DataAccess.Concrete;
using Core.Extensions;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.Blog;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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



        public List<BlogListDto> GetByCategory(int categoryId)
        {
            using var context = new BlogContext();
            var result = (from blog in context.Blogs
                          join slug in context.Slugs
                          on new { EntityId = blog.BlogId, EntityType = "Blog" }
                          equals new { slug.EntityId, slug.EntityType }
                          where blog.Status &&  blog.CategoryId == categoryId
                           orderby blog.CategoryId descending
                          select new
                          {
                              blog.BlogId,
                              blog.Title,
                              blog.Image,
                              blog.AddedTime,
                              blog.Status,
                              blog.CategoryId,
                              blog.ViewCount,
                              CategoryName = blog.Category.CategoryName,
                              Description = blog.Description,
                              Slug = slug.SlugText
                          })
                        .AsNoTracking()
                   .ToList();
            return result.Select(b => new BlogListDto
            {
                BlogId = b.BlogId,
                Title = b.Title,
                Image = b.Image,
                AddedTime = b.AddedTime,
                ViewCount = b.ViewCount,
                CategoryId = b.CategoryId,
                CategoryName = b.CategoryName,
                Status = b.Status,
                Slug = b.Slug,
                ShortDescription = b.Description.ToShort(200)
            }).ToList();
        }
        
        public List<BlogListDto> GetAllBlog()
        {
               using var context=new BlogContext();
               var result=(from blog in  context.Blogs
                           join slug in context.Slugs
                           on new {EntityId=blog.BlogId,EntityType="Blog"}
                           equals new {slug.EntityId,slug.EntityType}
                           where blog.Status
                           orderby blog.BlogId descending
                           select new
                           {
                               blog.BlogId,
                               blog.Title,
                               blog.Image,
                               blog.AddedTime,
                               blog.Status,
                               blog.CategoryId,
                               blog.ViewCount,
                               CategoryName=blog.Category.CategoryName,
                               Description=blog.Description,
                               Slug=slug.SlugText  
                           })
                           .AsNoTracking()
                      .ToList();
            return result.Select(b=> new BlogListDto {
                BlogId = b.BlogId,
                Title = b.Title,
                Image = b.Image,
                AddedTime = b.AddedTime,
                ViewCount = b.ViewCount,
                CategoryId = b.CategoryId,
                CategoryName = b.CategoryName,
                Status = b.Status,
                Slug = b.Slug,
                ShortDescription = b.Description.ToShort(200)
            }).ToList();
        }

        public Blog GetBlogDetail(string slug)
        {
            using var context = new BlogContext();
            var slugEntity = context.Slugs
        .AsNoTracking()
        .FirstOrDefault(s => s.SlugText == slug && s.EntityType == "Blog");

            if (slugEntity == null)
                return null;

            return context.Blogs
                .AsNoTracking()
                .FirstOrDefault(b => b.BlogId == slugEntity.EntityId && b.Status);
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
            var result = (from blog in context.Blogs
                          join slug in context.Slugs
                          on new { EntityId = blog.BlogId, EntityType = "Blog" }
                          equals new { slug.EntityId, slug.EntityType }
                          where blog.Status
                          orderby blog.ViewCount descending
                          select new
                          {
                              blog.BlogId,
                              blog.Title,
                              blog.Image,
                              blog.AddedTime,
                              blog.Status,
                              blog.ViewCount,
                              blog.Category.CategoryName,
                              blog.CategoryId,
                              Description = blog.Description ?? "",
                              Slug = slug.SlugText
                          })
                  .Take(50)
                  .AsNoTracking()
                  .ToList();

            return result.Select(b => new BlogListDto
            {
                BlogId = b.BlogId,
                Title = b.Title,
                Image = b.Image,
                AddedTime = b.AddedTime,
                ViewCount = b.ViewCount,
                CategoryName = b.CategoryName,
                CategoryId = b.CategoryId,
                Status = b.Status,
                Slug = b.Slug,
                ShortDescription = b.Description.ToShort(200)
            }).ToList();
        }



        public List<BlogListDto> GetLastTenBlog()
        {
            using var context = new BlogContext();
            var result = (from blog in context.Blogs
                          join slug in context.Slugs
                          on new { EntityId = blog.BlogId, EntityType = "Blog" }
                          equals new { slug.EntityId, slug.EntityType }
                          where blog.Status
                          orderby blog.BlogId descending
                          select new
                          {
                              blog.BlogId,
                              blog.Title,
                              blog.Image,
                              blog.AddedTime,
                              blog.Status,
                              blog.ViewCount,
                              blog.Category.CategoryName,
                              blog.CategoryId,
                              Description = blog.Description ?? "",
                              Slug = slug.SlugText
                          })
                  .Take(10)
                  .AsNoTracking()
                  .ToList();

            return result.Select(b => new BlogListDto
            {
                BlogId = b.BlogId,
                Title = b.Title,
                Image = b.Image,
                AddedTime = b.AddedTime,
                ViewCount = b.ViewCount,
                CategoryName = b.CategoryName,
                CategoryId = b.CategoryId,
                Status = b.Status,
                Slug = b.Slug,
                ShortDescription = b.Description.ToShort(200)
            }).ToList();
        }

        public List<BlogListDto> GetAdmin50Blog()
        {
            using var context = new BlogContext();
            var result = (from blog in context.Blogs
                          join slug in context.Slugs
                          on new { EntityId = blog.BlogId, EntityType = "Blog" }
                          equals new { slug.EntityId, slug.EntityType }
                          orderby blog.BlogId descending
                          select new
                          {
                              blog.BlogId,
                              blog.Title,
                              blog.Image,
                              blog.AddedTime,
                              blog.Status,
                              blog.ViewCount,
                              blog.Category.CategoryName,
                              blog.CategoryId,
                              Description = blog.Description ?? "",
                              Slug = slug.SlugText
                          })
                  .Take(50)
                  .AsNoTracking()
                  .ToList();

            return result.Select(b => new BlogListDto
            {
                BlogId = b.BlogId,
                Title = b.Title,
                Image = b.Image,
                AddedTime = b.AddedTime,
                ViewCount = b.ViewCount,
                CategoryName = b.CategoryName,
                CategoryId = b.CategoryId,
                Status = b.Status,
                Slug = b.Slug,
                ShortDescription = b.Description.ToShort(200)
            }).ToList();
        }

        public List<Blog> GetAllWithCategory(Expression<Func<Blog, bool>> filter = null)
        {
            using var ctx = new BlogContext();
            IQueryable<Blog> q = ctx.Blogs.Include(b => b.Category);
            if (filter != null) q = q.Where(filter);
            return q.OrderByDescending(b => b.BlogId).ToList();
        }
    }
}
