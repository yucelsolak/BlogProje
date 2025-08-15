using Core.Entities;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Blog
{
    public class AddUpdateBlogDto:IDto
    {
        public int BlogId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; } 
        public int ViewCount { get; set; } 
        public DateTimeOffset AddedTime { get; set; } 

        public string Slug { get; set; }
        public IFormFile BlogImage { get; set; }
        public string Keywords { get; set; }
    }
}
