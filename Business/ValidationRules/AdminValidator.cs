using Entities.DTOs.Admin;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules
{
    public class AdminValidator: AbstractValidator<AddUpdateAdmin>
    {
        public AdminValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Ad Soyad Alanı Boş Geçilemez");
            RuleFor(p => p.Email)
                        .NotEmpty().WithMessage("Email Alanı Boş Geçilemez")
                        .EmailAddress().WithMessage("Geçerli bir email giriniz.");

            // ADD: şifre zorunlu + karmaşıklık
            When(p => p.AdminId == 0, () =>
            {
                RuleFor(p => p.Password)
                    .NotEmpty().WithMessage("Şifre boş geçilemez.")
                    .MinimumLength(6).WithMessage("Şifre en az 6 karakter.")
                    .Matches("[A-Z]").WithMessage("Şifre en az 1 büyük harf içermeli.")
                    .Matches(@"\d").WithMessage("Şifre en az 1 rakam içermeli.")
                    .Matches(@"[^a-zA-Z0-9]").WithMessage("Şifre en az 1 özel karakter içermeli.");
            });

            // UPDATE: şifre opsiyonel; girildiyse karmaşıklık
            When(p => p.AdminId != 0, () =>
            {
                RuleFor(p => p.Password).Cascade(CascadeMode.Stop)
                    .MinimumLength(6).When(x => !string.IsNullOrWhiteSpace(x.Password)).WithMessage("Şifre en az 6 karakter.")
                    .Matches("[A-Z]").When(x => !string.IsNullOrWhiteSpace(x.Password)).WithMessage("Şifre en az 1 büyük harf içermeli.")
                    .Matches(@"\d").When(x => !string.IsNullOrWhiteSpace(x.Password)).WithMessage("Şifre en az 1 rakam içermeli.")
                    .Matches(@"[^a-zA-Z0-9]").When(x => !string.IsNullOrWhiteSpace(x.Password)).WithMessage("Şifre en az 1 özel karakter içermeli.");
            });

        }
    }
}
