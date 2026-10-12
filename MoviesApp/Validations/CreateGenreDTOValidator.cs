using FluentValidation;
using MoviesApp.DTOs;
using MoviesApp.Repository;

namespace MoviesApp.Validations
{
    public class CreateGenreDTOValidator : AbstractValidator<CreateGenreDTO>
    {
        public CreateGenreDTOValidator(IGenresRepository genresRepository,IHttpContextAccessor httpContextAccessor)
        {
            var routeValueId = httpContextAccessor.HttpContext!.Request.RouteValues["id"];
            var id = 0;

            if(routeValueId is string routeValueIdString)
            {
                int.TryParse(routeValueIdString, out id);
            }

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ValidationUtilities.NonEmptyMessage)
                .MaximumLength(150).WithMessage(ValidationUtilities.MaximumLengthMessage)
                .Must(ValidationUtilities.FirstLetterUpperCase).WithMessage(ValidationUtilities.FirstLetterIsUpperCaseMessage)
                .MustAsync(async (name, _) =>
                {
                    var exists = await genresRepository.Exists(id, name: name);
                    return !exists;
                }).WithMessage("The {PropertyName} must be unique.");
        }

       
    }
}
