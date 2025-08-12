using Entities.DTOs.Category;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules
{
    public class CategoryValidator:AbstractValidator<UpdateCategoryDto>
    {
        public CategoryValidator()
        {
            RuleFor(p => p.CategoryName).NotEmpty().WithMessage("Kategori Adı Boş Geçilemez");

        }
    }
}
