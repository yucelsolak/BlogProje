using Entities.Concrete;
using Entities.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IBlogCategoryService:IGenericService<BlogCategory>
    {
        List<CategoryWithBlogCountDto> GetCategoriesWithBlogCount();
        BlogCategory? GetBySlug(string slug);
        void AddCategory(AddCategoryDto dto);
        void CategoryWithSlugUpdate(BlogCategory entity, string slugText);

        public bool CanDeleteCategory(int categoryId);
    }
}
