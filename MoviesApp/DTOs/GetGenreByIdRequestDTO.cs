using AutoMapper;
using MoviesApp.Repository;

namespace MoviesApp.DTOs
{
    public class GetGenreByIdRequestDTO
    {
        public IGenresRepository Repository { get; set; } = null!;
        public int Id { get; set; }
        public IMapper Mapper { get; set; } = null!;
    }
}
