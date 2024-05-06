using PizzaPlace.Models;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Repositories;

public class StockRepository : IStockRepository
{
    public Task<StockDto> AddToStock(StockDto stock) => throw new NotImplementedException("A real repository must be implemented.");
    public Task<StockDto> GetStock(StockType stockType) => throw new NotImplementedException("A real repository must be implemented.");
    public Task<StockDto> TakeStock(StockType stockType, int amount) => throw new NotImplementedException("A real repository must be implemented.");
}
