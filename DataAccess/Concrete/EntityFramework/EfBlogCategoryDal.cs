using Core.DataAccess.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfBlogCategoryDal:EfEntityRepositoryBase<BlogCategory,BlogContext>,IBlogCategoryDal
    {
        public BlogCategory GetBySlug(string slug)
        {
            using var context = new BlogContext();
            return context.BlogCategories.AsNoTracking()
                .FirstOrDefault(c => c.Slug == slug && c.Status);
        }

        public List<CategoryWithBlogCountDto> GetCategoriesWithBlogCount()
        {
            using (var context = new BlogContext())
            {
                return context.BlogCategories
                    .Select(c => new CategoryWithBlogCountDto
                    {
                        CategoryId = c.CategoryId,
                        CategoryName = c.CategoryName,
                        BlogCount = c.Blogs.Count(),
                        Slug = c.Slug
                    }).ToList();
            }
        }
    }
}
