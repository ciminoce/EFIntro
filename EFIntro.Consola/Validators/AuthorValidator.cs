using EFIntro.Entities;
using FluentValidation;

namespace EFIntro.Consola.Validators
{
    public class AuthorValidator : AbstractValidator<Author>
    {
        public AuthorValidator()
        {
            RuleFor(a => a.FirstName)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.")
                .MaximumLength(50).WithMessage("El nombre no puede tener más de 50 caracteres.");

            RuleFor(a => a.LastName)
                .NotEmpty().WithMessage("El apellido es obligatorio.")
                .MinimumLength(3).WithMessage("El apellido debe tener al menos 3 caracteres.")
                .MaximumLength(50).WithMessage("El apellido no puede tener más de 50 caracteres.");

        }
    }
}
