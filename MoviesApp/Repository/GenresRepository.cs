using Dapper;
using Microsoft.Data.SqlClient;
using MoviesApp.Entities;
using System.Data;

namespace MoviesApp.Repository
{
    public class GenresRepository : IGenresRepository
    {
        private readonly string _connectionString;

        public GenresRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        public async Task<int> Create(Genre genre)
        {
            using (var connection = new SqlConnection(_connectionString))
            {                
                var id = await connection.QuerySingleAsync<int>("Genres_Create", 
                    new { genre.Name}, commandType: CommandType.StoredProcedure);
                genre.Id = id;

                return id;
            }
            
        }        

        public async Task<bool> Exists(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var exists = await connection.QuerySingleAsync<bool>(
                    @"Genres_Exists", new {id},commandType:CommandType.StoredProcedure);

                return exists;
            }
        }

        public async Task<List<Genre>> GetAll()
        {
            //instantiate connection
            using (var connection = new SqlConnection(_connectionString))
            {
                var genres = await connection.QueryAsync<Genre>(@"Genres_GetAll",commandType:CommandType.StoredProcedure);

                return genres.ToList();
            }
        }

        public async Task<Genre?> GetById(int id)
        {
            using(var con = new SqlConnection(_connectionString))
            {
                var genre = await con.QueryFirstOrDefaultAsync<Genre>(
                    @"
                    Genres_GetById
                    ", new {id},commandType:CommandType.StoredProcedure);

                return genre;
            }
        }

        public async Task Update(Genre genre)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(
                    "Genres_Update", new {genre.Id, genre.Name},commandType:CommandType.StoredProcedure);
            }
        }

        public async Task Delete(int id)
        {
            using(var connection  = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(
                    "Genres_Delete", new {id}, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
