using MoviesApp.Entities;

namespace MoviesApp.Repository
{
    public interface IErrorsRepository
    {
        Task<Guid> Create(Error error);
    }
}