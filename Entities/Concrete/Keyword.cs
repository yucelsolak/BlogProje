using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Keyword:IEntity
    {
        [Key]
        public int KeywordId { get; set; }
        public string KeywordName { get; set; }
        public ICollection<KeywordBlog> KeywordBlogs { get; set; } = new List<KeywordBlog>(); // ← başlat
    }
}
