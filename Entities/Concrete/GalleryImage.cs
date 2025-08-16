using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class GalleryImage
    {
        public int GalleryImageId { get; set; }
        public int PhotoGalleryId { get; set; }
        public PhotoGallery PhotoGallery { get; set; }
        public int SortOrder { get; set; }         // sıralama için
    }
}
