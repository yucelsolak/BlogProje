using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Category
{
    public class AddCategoryDto
    {
        [Required]
        public string CategoryName { get; set; } = null!;
        public bool Status { get; set; } = true;
        public string Slug { get; set; }
    }
}
