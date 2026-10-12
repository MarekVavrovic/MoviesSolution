using FluentValidation;
using MoviesApp.DTOs;

namespace MoviesApp.Validations
{
    public class CreateMovieDTOValidator : AbstractValidator<CreateMovieDTO>
    {
        public CreateMovieDTOValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage(ValidationUtilities.NonEmptyMessage)
                .MaximumLength(250).WithMessage(ValidationUtilities.MaximumLengthMessage);
        }
    }
}
