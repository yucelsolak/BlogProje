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
    public interface IBlogCategoryDal:IEntityRepository<BlogCategory>
    {
        List<CategoryWithBlogCountDto> GetCategoriesWithBlogCount();
        BlogCategory? GetBySlug(string slug);
    }
}
