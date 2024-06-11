using Microsoft.EntityFrameworkCore;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Repositories
{
    public class StockRepository : IStockRepository
    {
        private static readonly PizzaContext s_dbContext = new();
        private static readonly object s_lock = new();

        public async Task<Ingredient> AddToStock(Ingredient stock)
        {
            if (stock.Amount < 0)
            {
                throw new PizzaException("Stock cannot have negative amount.");
            }

            Ingredient? existingStock =
                await s_dbContext.Stock.FirstOrDefaultAsync(
                    s => s.IngredientType == stock.IngredientType);

            if (existingStock != null)
            {
                existingStock.Amount += stock.Amount;
            }
            else
            {
                await s_dbContext.AddAsync(stock);
                existingStock = stock;
            }
            
            await s_dbContext.SaveChangesAsync();

            return existingStock;
        }

        public async Task<Ingredient?> GetStock(IngredientType ingredientType)
        {
            Ingredient? stock =
                await s_dbContext.Stock.FirstOrDefaultAsync(
                    s => s.IngredientType == ingredientType);

            return stock;
        }

        public async Task<Ingredient> TakeStock(IngredientType stockType, int amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(amount), "Unable to take zero or negative amount.");
            }

            Ingredient? stock = await GetStock(stockType);
            if (stock == null || stock.Amount < amount)
                throw new PizzaException("Not enough stock to take the given amount.");

            lock (s_lock)
            {
                stock.Amount -= amount;
            }
            await s_dbContext.SaveChangesAsync();

            return new Ingredient(stockType, amount);
        }
    }
}
