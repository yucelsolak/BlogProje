using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class KeywordBlog:IEntity
    {
        [Key]
        public int KeywordBlogId { get; set; }
        public int KeywordId { get; set; }
        public Keyword Keyword { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
