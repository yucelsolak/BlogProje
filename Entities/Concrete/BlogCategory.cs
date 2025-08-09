using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class BlogCategory : IEntity
    {
        [Key]
        public int CategoryId { get; set; }
        [Required, MaxLength(100)]
        public string CategoryName { get; set; } = null!;
        public bool Status { get; set; } = true; 
        public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
    }
}
