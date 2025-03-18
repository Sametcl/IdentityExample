using FluentValidation;
using IdentitiyExample.Models;
using IdentityExample.DTOs;

namespace IdentityExample.Validators
{
    public class RoleCreateValidator : AbstractValidator<RoleCreateDto>
    {
        public RoleCreateValidator()
        {
            RuleFor(x=>x.roleName).MinimumLength(3).WithMessage("Lutfen bos birakmayiniz ve 3 karakterden uzun rol adi giriniz  ");
        }
    }
}
