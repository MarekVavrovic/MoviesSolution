using AutoMapper;
using MoviesApp.Repository;

namespace MoviesApp.Filters
{
    public class TestFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            //code executing before the endpoint executes
            var param1 = context.Arguments.OfType<int>().FirstOrDefault();
            var param2 = context.Arguments.OfType<IGenresRepository>().FirstOrDefault();
            var param3 = context.Arguments.OfType<IMapper>().FirstOrDefault();

            var result = await next(context);

            //code executing after the endpoint executes
            return result;
        }
    }
}
