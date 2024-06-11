using PizzaPlace.Models;

namespace PizzaPlace.Services
{
    public interface IStockService
    {
        Task<bool> HasInsufficientStock(
            PizzaOrder order, ComparableList<Recipe> recipeDtos);

        Task<ComparableList<Ingredient>> GetStock(
            PizzaOrder order, ComparableList<Recipe> recipeDtos);
    }
}
