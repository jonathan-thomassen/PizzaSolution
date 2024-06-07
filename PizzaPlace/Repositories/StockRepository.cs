using Microsoft.EntityFrameworkCore;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Repositories
{
    public class StockRepository : IStockRepository
    {
        private static readonly PizzaContext s_dbContext = new();
        private static readonly object s_lock = new();

        public async Task<Stock> AddToStock(Stock stock)
        {
            await s_dbContext.AddAsync(stock);
            await s_dbContext.SaveChangesAsync();

            return stock;
        }

        public async Task<Stock?> GetStock(StockType stockType)
        {
            Stock? stock =
                await s_dbContext.Stock.FirstOrDefaultAsync(s => s.StockType == stockType);

            return stock;
        }

        public async Task<Stock> TakeStock(StockType stockType, int amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(amount), "Unable to take zero or negative amount.");
            }

            Stock? stock = await GetStock(stockType);
            if (stock == null || stock.Amount < amount)
                throw new PizzaException("Not enough stock to take the given amount.");

            Stock updatedStock = stock with { Amount = stock.Amount - amount };
            lock (s_lock)
            {
                s_dbContext.Entry(stock).CurrentValues.SetValues(updatedStock);
            }

            return stock with { Amount = amount, Id = 0 };
        }
    }
}
