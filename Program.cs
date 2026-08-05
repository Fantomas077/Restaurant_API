using Microsoft.EntityFrameworkCore;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.IRepositories;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<RestaurantsDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Defaultconnection")));
builder.Services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();
builder.Services.AddScoped<IRestaurantsService, RestaurantsService>();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<RestaurantsProfile>();
    cfg.AddProfile<DishesProfile>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
