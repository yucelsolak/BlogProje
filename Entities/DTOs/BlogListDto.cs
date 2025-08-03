using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class BlogListDto : IDto
    {
        public int BlogId { get; set; }
        public string Title { get; set; } = null!;
        public string ShortDescription { get; set; } = null!;
        public string? Image { get; set; }
        public string BlogSlug { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public int ViewCount { get; set; } = 0;
        public string CategoryName {  get; set; }
        public int CategoryId {  get; set; }
        public bool Status  { get; set; }
        public DateTimeOffset AddedTime { get; set; }
    }
}
