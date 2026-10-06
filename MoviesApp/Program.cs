using MoviesApp.Endpoints;
using MoviesApp.Repository;
using MoviesApp.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(configuration =>
    {
        configuration.WithOrigins(builder.Configuration["allowedOrigins"]!).AllowAnyMethod()
        .AllowAnyHeader();
    });

    options.AddPolicy("free", configuration =>
    {
        configuration.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });

});
builder.Services.AddOutputCache();

builder.Services.AddScoped<IGenresRepository, GenresRepository>();
builder.Services.AddScoped<IActorsRepository, ActorsRepository>();
builder.Services.AddScoped<IMoviesRepository, MoviesRepository>();
builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();
//Azure Storage
//builder.Services.AddTransient<IFileStorage, AzureFileStorage>();

//Local Storage
builder.Services.AddTransient<IFileStorage, LocalFileStorage>();
builder.Services.AddHttpContextAccessor();

//AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

//swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();

app.UseCors();
app.UseOutputCache();

app.MapGet("/", () => "Hello World!");

//Map endpoints
app.MapGroup("/genres").MapGenres();
app.MapGroup("/actors").MapActors();
app.MapGroup("/movies").MapMovies();
app.MapGroup("movies/{movieId:int}/comments").MapComments();


app.Run();











