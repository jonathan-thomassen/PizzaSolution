using PizzaPlace.Factories;
using PizzaPlace.Repositories;
using PizzaPlace.Services;
using PizzaPlace.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;

services.AddCors(o =>
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

services.AddControllers();
services.AddOpenApiDocument(d =>
{
    d.Title = "Pizza Place";
    d.Version = "v1";
});

services.AddDbContext<StockDtoDBContext>();
services.AddDbContext<RecipeDtoDBContext>();

services.AddSingleton(TimeProvider.System);

services.AddTransient<IStockRepository, FakeStockRepository>();
services.AddTransient<IRecipeRepository, RecipeRepository>();

services.AddTransient<IPizzaOven, NormalPizzaOven>();
services.AddTransient<IPizzaOven, AssemblyLinePizzaOven>();
services.AddTransient<IPizzaOven, GiantRevolvingPizzaOven>();

services.AddTransient<IStockService, StockService>();
services.AddTransient<IRecipeService, RecipeService>();
services.AddTransient<IOrderingService, OrderingService>();
services.AddTransient<IMenuService, MenuService>();


WebApplication app = builder.Build();

app.UseOpenApi();
app.UseSwaggerUi();
app.UseDeveloperExceptionPage();

app.UseCors();

app.MapControllers();

app.Run();
