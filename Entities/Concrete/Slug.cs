using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Slug:IEntity
    {
        [Key]
        public int SlugId { get; set; }
        public string SlugText { get; set; } // örn: urunadi, urunadi-1
        public string EntityType { get; set; } // örn: Product, Blog, News
        public int EntityId { get; set; }
    }

}
