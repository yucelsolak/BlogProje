using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Blog:IEntity
    {
        [Key]
        public int BlogId { get; set; }
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        [Required, MaxLength(300)]
        public string Title { get; set; }
        [Required, MaxLength(400)]
        public string Slug { get; set; } = null!;
        [Required]
        public string Description { get; set; }
        [MaxLength(300)]
        public string Image { get; set; }
        public bool Status { get; set; } = true;
        public int ViewCount { get; set; } = 0;
        public DateTimeOffset AddedTime { get; set; } = DateTimeOffset.UtcNow;

        public virtual BlogCategory Category { get; set; } = null!;

    }
}
