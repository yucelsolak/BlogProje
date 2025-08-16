using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Blog
{
    public class KeywordLinkDto
    { 
        public int KeywordId { get; set; }
        public string KeywordName { get; set; } 
        public string Slug { get; set; }
    }
}
