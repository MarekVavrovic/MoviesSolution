using Microsoft.AspNetCore.Identity;

namespace MoviesApp.Services
{
    public interface IUsersService
    {
        Task<IdentityUser?> GetUser();
    }
}