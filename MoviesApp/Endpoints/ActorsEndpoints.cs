using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MoviesApp.DTOs;
using MoviesApp.Entities;
using MoviesApp.Filters;
using MoviesApp.Repository;
using MoviesApp.Services;


namespace MoviesApp.Endpoints
{
    public static class ActorsEndpoints
    {
        private readonly static string container = "actors";
        public static RouteGroupBuilder MapActors(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAll).CacheOutput(c => c.Expire(TimeSpan.FromMinutes(1)).Tag("actors-get"));
            group.MapGet("/{id:int}", GetById);
            group.MapGet("getByName/{name}", GetByName);

            group.MapPut("/{id:int}", Update).DisableAntiforgery().AddEndpointFilter<ValidationFilter<CreateActorDTO>>().RequireAuthorization("isadmin");
            group.MapDelete("/{id:int}", Delete).DisableAntiforgery().RequireAuthorization("isadmin");
            group.MapPost("/", Create).DisableAntiforgery().AddEndpointFilter<ValidationFilter<CreateActorDTO>>().RequireAuthorization("isadmin");

            return group;
        }

        static async Task<Created<ActorDTO>> Create([FromForm] CreateActorDTO createActorDTO,
           IActorsRepository repository, IOutputCacheStore outputCacheStore,
           IMapper mapper, IFileStorage fileStorage)
        {           
            var actor = mapper.Map<Actor>(createActorDTO);

            //Local Storage - wwwroot/actors
            if (createActorDTO.Picture is not null)
            {
                var url = await fileStorage.Store(container, createActorDTO.Picture);
                actor.Picture = url;
            }

            var id = await repository.Create(actor);
            await outputCacheStore.EvictByTagAsync("actors-get", default);
            var actorDTO = mapper.Map<ActorDTO>(actor);
            return TypedResults.Created($"/actors/{id}", actorDTO);
        }

        static async Task<Ok<List<ActorDTO>>> GetAll(IActorsRepository repository, IMapper mapper, int page = 1, int recordsPerPage = 10)
        {
            var pagination = new PaginationDTO() { Page = page, RecordsPerPage = recordsPerPage };
            var actors = await repository.GetAll(pagination);
            var actorsDTO = mapper.Map<List<ActorDTO>>(actors);
            return TypedResults.Ok(actorsDTO);
        }

        static async Task<Results<Ok<ActorDTO>, NotFound>> GetById(int id, IActorsRepository repository, IMapper mapper)
        {
            var actor = await repository.GetById(id);

            if (actor is null)
            {
                return TypedResults.NotFound();
            }

            var actorDTO = mapper.Map<ActorDTO>(actor);
            return TypedResults.Ok(actorDTO);
        }

        static async Task<Ok<List<ActorDTO>>> GetByName(string name, IActorsRepository repository, IMapper mapper)
        {
            var actors = await repository.GetByName(name);
            var actorsDTO = mapper.Map<List<ActorDTO>>(actors);
            return TypedResults.Ok(actorsDTO);
        }

        static async Task<Results<NoContent, NotFound>> Update(int id, [FromForm] CreateActorDTO createActorDTO, IActorsRepository repository,
            IMapper mapper, IFileStorage fileStorage, IOutputCacheStore outputCacheStore)
        {
            var actor = await repository.GetById(id);
            if (actor is null)
            {
                return TypedResults.NotFound();
            }

            var actorForUpdate = mapper.Map<Actor>(createActorDTO);
            actorForUpdate.Id = id;
            actorForUpdate.Picture = actor.Picture;

            if (createActorDTO.Picture is not null)
            {
                var url = await fileStorage.Edit(actor.Picture, container, createActorDTO.Picture);
                actorForUpdate.Picture = url;
            }

            await repository.Update(actorForUpdate);
            await outputCacheStore.EvictByTagAsync("actors-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> Delete(int id, IActorsRepository repository, IOutputCacheStore outputCacheStore ,IFileStorage fileStorage)
        {
            var actor = await repository.GetById(id);
            if (actor is null)
            {
                return TypedResults.NotFound();
            }
            await repository.Delete(id);
            await fileStorage.Delete(actor.Picture, container);
            await outputCacheStore.EvictByTagAsync("actors-get", default);

            return TypedResults.NoContent();
        }
    }
}
