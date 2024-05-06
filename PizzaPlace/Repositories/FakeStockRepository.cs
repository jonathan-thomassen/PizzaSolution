using PizzaPlace.Models;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Repositories;

public class FakeStockRepository : FakeDatabase<StockDto>, IStockRepository
{
    private static readonly object _lock = new();

    public Task<StockDto> AddToStock(StockDto stock)
    {
        if (stock.Amount < 0)
            throw new PizzaException("Stock cannot have negative amount.");

        lock (_lock)
        {
            var currentStocks = Get(x => x.StockType == stock.StockType);
            if (currentStocks.FirstOrDefault() is StockDto existingStock)
            {
                var updatedStock = existingStock with { Amount = existingStock.Amount + stock.Amount };
                Update(updatedStock, updatedStock.Id);

                return Task.FromResult(updatedStock);
            }
            else
            {
                _ = Insert(stock);
                return Task.FromResult(stock);
            }
        }
    }

    public Task<StockDto> GetStock(StockType stockType) =>
        Task.FromResult(Get(x => x.StockType == stockType)
            .FirstOrDefault(new StockDto(stockType, 0)));

    public Task<StockDto> TakeStock(StockType stockType, int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Unable to take zero or negative amount.");

        lock (_lock)
        {
            var stock = GetStock(stockType).GetAwaiter().GetResult();
            if (stock.Amount < amount)
                throw new PizzaException("Not enough stock to take the given amount.");

            var updatedStock = stock with { Amount = stock.Amount - amount };
            Update(updatedStock, updatedStock.Id);

            return Task.FromResult(stock with { Amount = amount, Id = 0 });
        }
    }
}
