using PizzaPlace.Models;

namespace PizzaPlace.Services
{
    public interface IStockService
    {
        Task<bool> HasInsufficientStock(
            PizzaOrder order, ComparableList<PizzaRecipe> recipeDtos);

        Task<ComparableList<Stock>> GetStock(
            PizzaOrder order, ComparableList<PizzaRecipe> recipeDtos);
    }
}
