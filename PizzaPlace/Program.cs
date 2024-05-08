using PizzaPlace.Factories;
using PizzaPlace.Repositories;
using PizzaPlace.Services;
using Microsoft.EntityFrameworkCore;
using PizzaPlace.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o =>
{
    o.AddPolicy("allowAll",
        policy =>
        {
            policy.WithOrigins(
            // "whatEver.homepage.com"
            );
            policy.AllowAnyHeader();
            policy.AllowCredentials();
            policy.AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddOpenApiDocument(d =>
{
    d.Title = "Pizza Place";
    d.Version = "v1";
});

builder.Services.AddDbContext<StockDtoDBContext>();
builder.Services.AddDbContext<RecipeDtoDBContext>();

// Register services:
var services = builder.Services;
services.AddSingleton(TimeProvider.System);

services.AddTransient<IStockRepository, FakeStockRepository>();
services.AddTransient<IRecipeRepository, FakeRecipeRepository>();

services.AddTransient<IPizzaOven, NormalPizzaOven>();
services.AddTransient<IPizzaOven, AssemblyLinePizzaOven>();
services.AddTransient<IPizzaOven, GiantRevolvingPizzaOven>();

services.AddTransient<IStockService, StockService>();
services.AddTransient<IRecipeService, RecipeService>();
services.AddTransient<IOrderingService, OrderingService>();
services.AddTransient<IMenuService, MenuService>();


var app = builder.Build();

app.UseOpenApi();
app.UseSwaggerUi();
app.UseDeveloperExceptionPage();

app.UseCors();

app.MapControllers();

app.Run();
