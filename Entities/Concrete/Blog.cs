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
        public string Title { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; } = true;
        public int ViewCount { get; set; } = 0;
        public DateTimeOffset AddedTime { get; set; } = DateTimeOffset.UtcNow;

        public virtual BlogCategory Category { get; set; } = null!;

    }
}
