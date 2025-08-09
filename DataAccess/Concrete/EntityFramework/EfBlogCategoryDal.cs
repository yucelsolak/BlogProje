using Core.DataAccess.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.Category;
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
            var categorySlug = context.Slugs
        .FirstOrDefault(s => s.SlugText == slug && s.EntityType == "Category");

            if (categorySlug == null)
                return null;

            return context.BlogCategories
                .AsNoTracking()
                .FirstOrDefault(c => c.CategoryId == categorySlug.EntityId && c.Status);
        }

        public List<CategoryWithBlogCountDto> GetCategoriesWithBlogCount()
        {
            using (var context = new BlogContext())
            {
                var result = from category in context.BlogCategories
                             join slug in context.Slugs
                             on new { EntityId = category.CategoryId, EntityType = "Category" }
                             equals new { slug.EntityId, slug.EntityType }
                             select new CategoryWithBlogCountDto
                             {
                                 CategoryId = category.CategoryId,
                                 CategoryName = category.CategoryName,
                                 BlogCount = category.Blogs.Count(),
                                 Slug = slug.SlugText 
                             };

                return result.ToList();
            }
        }
    }
}
