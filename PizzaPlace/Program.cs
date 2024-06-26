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

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddDbContext<PizzaContext>();

services.AddSingleton(TimeProvider.System);

services.AddTransient<IStockRepository, StockRepository>();
services.AddTransient<IRecipeRepository, RecipeRepository>();

services.AddTransient<IPizzaOven, NormalPizzaOven>();
services.AddTransient<IPizzaOven, AssemblyLinePizzaOven>();
services.AddTransient<IPizzaOven, GiantRevolvingPizzaOven>();

services.AddTransient<IStockService, StockService>();
services.AddTransient<IRecipeService, RecipeService>();
services.AddTransient<IOrderingService, OrderingService>();
services.AddTransient<IMenuService, MenuService>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseDeveloperExceptionPage();

app.UseCors();
app.MapControllers();
app.Run();
