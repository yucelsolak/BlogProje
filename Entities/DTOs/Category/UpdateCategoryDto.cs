using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Category
{
    public class UpdateCategoryDto:IDto
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public bool Status { get; set; }
        public string Slug { get; set; }
    }
}
