using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Category
{
    public class CategoryWithBlogCountDto:IDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int BlogCount { get; set; }
        public string Slug { get; set; }
    }
}
