using FluentValidation;
using MoviesApp.DTOs;

namespace MoviesApp.Filters
{
    public class GenresValidationFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validator = context.HttpContext.RequestServices.GetService<IValidator<CreateGenreDTO>>();

            if (validator == null)
            {
                return await next(context);
            }

            //object we want to validate
            var obj= context.Arguments.OfType<CreateGenreDTO>().FirstOrDefault();

            if (obj == null)
            {
                return Results.Problem("No object to validate");
            }

            //validate the object - code from endpoints
            var validationResult = await validator.ValidateAsync(obj);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            return await next(context);
        }
    }
}
