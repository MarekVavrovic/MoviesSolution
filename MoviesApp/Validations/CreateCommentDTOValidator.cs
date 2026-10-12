using FluentValidation;
using MoviesApp.DTOs;

namespace MoviesApp.Validations
{
    public class CreateCommentDTOValidator : AbstractValidator<CreateCommentDTO>
    {
        public CreateCommentDTOValidator()
        {
            RuleFor(x => x.Body).NotEmpty().WithMessage(ValidationUtilities.NonEmptyMessage);
        }
    }
}
