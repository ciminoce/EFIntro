using EFIntro.Entities;
using FluentValidation;

namespace EFIntro.Consola.Validators
{
    public class BooksValidator : AbstractValidator<Book>
    {
        public BooksValidator()
        {
            RuleFor(b => b.Title).NotEmpty().WithMessage("The {PropertyName} is required")
                .MaximumLength(300).WithMessage("The {PropertyName} must have no more than {ComparisonValue} characters");

            RuleFor(b => b.Pages).NotEmpty().WithMessage("The {PropertyName} is required")
                .GreaterThan(0).WithMessage("The {PropertyName} must be greather than {ComparisonValue}");

            RuleFor(b => b.PublishDate).NotEmpty().WithMessage("The {PropertyName} is required")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today.Date)).WithMessage("The {PropertyName} must be at least {ComparisonValue}");
            When(b => b.AuthorId == 0, () =>
            {
                RuleFor(b => b.AuthorId).Equal(0).WithMessage("When adding a new Author, AuthorId must be {ComparisonValue}");
            }).Otherwise(() => {

                RuleFor(b => b.AuthorId).GreaterThan(0).WithMessage("The field {PropertyName} must be greater than {ComparisonValue}");
            });

        }
    }
}
