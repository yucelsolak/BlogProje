using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class PhotoGallery:IEntity
    {
        public int PhotoGalleryId { get; set; }
        public string Title { get; set; }

        // Blog’a bağlı olabilir de olmayabilir de
        public int? BlogId { get; set; }
        public Blog Blog { get; set; }

        public ICollection<GalleryImage> Images { get; set; } = new List<GalleryImage>();
    }
}
