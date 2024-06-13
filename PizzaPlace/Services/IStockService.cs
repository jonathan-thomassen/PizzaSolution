using PizzaPlace.Models;

namespace PizzaPlace.Services
{
    public interface IStockService
    {
        Task<bool> HasInsufficientStock(
            PizzaOrder order, ComparableList<RecipeDto> recipeDtos);

        Task<ComparableList<StockDto>> GetStock(
            PizzaOrder order, ComparableList<RecipeDto> recipeDtos);
    }
}
