using Dapper;
using Microsoft.Data.SqlClient;
using MoviesApp.Entities;
using System.Data;

namespace MoviesApp.Repository
{
    public class ActorsRepository : IActorsRepository
    {
        private readonly string _connectionString;
        public ActorsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<int> Create(Actor actor)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var id = await connection.QuerySingleAsync<int>("Actors_Create",
                    new { actor.Name, actor.DateOfBirth, actor.Picture },
                    commandType: CommandType.StoredProcedure);

                actor.Id = id;
                return id;
            }
        }

        public async Task<List<Actor>> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var actors = await connection.QueryAsync<Actor>("Actors_GetAll",
                    commandType: CommandType.StoredProcedure);

                return actors.ToList();
            }
        }

        public async Task<Actor?> GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var actor = await connection.QueryFirstOrDefaultAsync<Actor>("Actors_GetById",
                    new { id }, commandType: CommandType.StoredProcedure);
                return actor;
            }
        }

        public async Task<bool> Exist(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var exists = await connection.QuerySingleAsync<bool>("Actors_Exist",
                    new { id }, commandType: CommandType.StoredProcedure);
                return exists;
            }
        }

        public async Task Update(Actor actor)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync("Actors_Update", new
                {
                    actor.Id,
                    actor.Name,
                    actor.DateOfBirth,
                    actor.Picture
                }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync("Actors_Delete",
                    new { id }, commandType: CommandType.StoredProcedure);
            }
        }

    }
}
