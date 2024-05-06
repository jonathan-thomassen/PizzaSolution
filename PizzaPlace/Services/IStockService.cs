using PizzaPlace.Models;

namespace PizzaPlace.Services;

public interface IStockService
{
    Task<bool> HasInsufficientStock(PizzaOrder order, ComparableList<PizzaRecipeDto> recipeDtos);

    Task<ComparableList<StockDto>> GetStock(PizzaOrder order, ComparableList<PizzaRecipeDto> recipeDtos);
}
