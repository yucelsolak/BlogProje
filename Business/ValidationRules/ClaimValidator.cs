
using Entities.DTOs.Claim;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules
{
    public class ClaimValidator:AbstractValidator<AddUpdateClaim>
    {
        public ClaimValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Görev Adı Boş Geçilemez");
        }
    }
}
