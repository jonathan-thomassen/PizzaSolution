using Microsoft.EntityFrameworkCore;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Repositories;

public class StockRepository : IStockRepository
{
    private static readonly PizzaContext s_dbContext = new PizzaContext();
    private static readonly object s_lock = new();

    public async Task<StockDto> AddToStock(StockDto stock)
    {
        await s_dbContext.AddAsync(stock);
        await s_dbContext.SaveChangesAsync();

        return stock;
    }

    public async Task<StockDto> GetStock(StockType stockType)
    {
        StockDto? stock = await s_dbContext.Stock.FirstOrDefaultAsync(
            s => s.StockType == stockType);

        return stock;
    }

    public async Task<StockDto> TakeStock(StockType stockType, int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(amount), "Unable to take zero or negative amount.");

        StockDto stock = await GetStock(stockType);
        if (stock.Amount < amount)
            throw new PizzaException(
                "Not enough stock to take the given amount.");

        StockDto updatedStock = stock with { Amount = stock.Amount - amount };
        lock (s_lock)
        {
            s_dbContext.Entry(stock).CurrentValues.SetValues(updatedStock);
        }

        return stock with { Amount = amount, Id = 0 };
    }
}
