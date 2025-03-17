using FluentValidation;
using IdentitiyExample.DTOs;

namespace IdentitiyExample.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(p=>p.UserName).MinimumLength(5).WithMessage("Ad alani en az 5 karakter olmali");
            RuleFor(p=>p.Email).MinimumLength(5).WithMessage("Email alani en az 5 karakter olmali");
        }
    }
}
