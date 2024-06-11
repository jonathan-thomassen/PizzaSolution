using PizzaPlace.Models;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Repositories
{
    public interface IStockRepository
    {
        Task<Ingredient> AddToStock(Ingredient stock);
        Task<Ingredient?> GetStock(IngredientType stockType);
        Task<Ingredient> TakeStock(IngredientType stockType, int amount);
    }
}
