using FluentValidation;
using MoviesApp.DTOs;

namespace MoviesApp.Validations
{
    public class EditClaimDTOValidator : AbstractValidator<EditClaimDTO>
    {
        public EditClaimDTOValidator()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage(ValidationUtilities.EmailAddressMessage);
        }
    }
}
