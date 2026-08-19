using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Restaurant.API.Middlewares;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Services;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Services;
using Restaurants.Domain.IRepositories;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ErrorHandlingMiddle>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<RestaurantsDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Defaultconnection")));
builder.Services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();
builder.Services.AddScoped<IRestaurantsService, RestaurantsService>();
builder.Services.AddScoped<IDishesRepository, DishesRepository>();
builder.Services.AddScoped<IDishesService, DishesService>();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<RestaurantsProfile>();
    cfg.AddProfile<DishesProfile>();
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience= false,
        ValidateLifetime= false,

        ValidateIssuerSigningKey= true,
        IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes("this-is-a-very-strong-secret-key-12345"))
    };

});
var app = builder.Build();
app.UseMiddleware<ErrorHandlingMiddle>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
