using EFIntro.Entities;
using FluentValidation;

namespace EFIntro.Consola.Validators
{
    public class AuthorValidator : AbstractValidator<Author>
    {
        public AuthorValidator()
        {
            RuleFor(a => a.FirstName)
                .NotEmpty().WithMessage("The name is required.")
                .MinimumLength(3).WithMessage("The name must have at least 3 characters")
                .MaximumLength(50).WithMessage("The name can not have more than 50 characters.");

            RuleFor(a => a.LastName)
                .NotEmpty().WithMessage("The surname is required.")
                .MinimumLength(3).WithMessage("The surname must have at least 3 characters")
                .MaximumLength(50).WithMessage("The surname can not have more than 50 characters.");

        }
    }
}
