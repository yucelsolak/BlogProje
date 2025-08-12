using Business.Constants;
using Entities.DTOs.Blog;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules
{
    public class BlogValidator: AbstractValidator<AddUpdateBlogDto>
    {
        public BlogValidator()
        {
            RuleFor(p => p.Title).NotEmpty().WithMessage("Blog Başlığı Boş Geçilemez");
            RuleFor(p => p.Description).NotEmpty().WithMessage("Blog İçeriği Boş Geçilemez");

            RuleFor(b => b.CategoryId)
            .NotNull().WithMessage("Bir Kategori Seçmelisiniz.");

            RuleFor(b => b.CategoryId)
            .GreaterThan(0).WithMessage("Bir Kategori Seçmelisiniz.");
        }
    }
}
