using MoviesApp.DTOs;
using MoviesApp.Entities;

namespace MoviesApp.Repository
{
    public interface IMoviesRepository
    {
        Task Assign(int id, List<int> genresIds);
        Task Assign(int id, List<ActorMovie> actors);
        Task<int> Create(Movie movie);
        Task Delete(int id);
        Task<bool> Exists(int id);
        Task<List<Movie>> GetAll(PaginationDTO paginationDTO);
        Task<Movie?> GetById(int id);
        Task Update(Movie movie);
        Task<List<Movie>> Filter(MoviesFilterDTO moviesFilterDTO);
    }
}