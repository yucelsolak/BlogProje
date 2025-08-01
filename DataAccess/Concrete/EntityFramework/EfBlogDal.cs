using Core.DataAccess.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
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
    }
}
