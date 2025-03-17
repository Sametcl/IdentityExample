using FluentValidation;
using IdentitiyExample.DTOs;

namespace IdentitiyExample.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(s=>s.EmailOrUserName).MinimumLength(5).WithMessage("5 karakterden uzun bir email ya da username giriniz");
        }
    }
}
