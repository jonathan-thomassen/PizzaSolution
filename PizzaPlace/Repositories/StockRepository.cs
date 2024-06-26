﻿using Microsoft.EntityFrameworkCore;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Repositories
{
    public class StockRepository : IStockRepository
    {
        private static readonly PizzaContext s_dbContext = new();
        private static readonly object s_lock = new();

        public async Task<StockDto?> AddToStock(StockDto stock)
        {
            if (stock.Amount < 0)
            {
                throw new PizzaException("Stock cannot have negative amount.");
            }

            StockDto? existing = await s_dbContext.StockDtos.FirstOrDefaultAsync(
                s => s.StockType == stock.StockType);

            if (existing != null)
            {
                StockDto newStock = existing with { Amount = existing.Amount + stock.Amount };
                lock (s_lock)
                {
                    s_dbContext.Entry(existing).CurrentValues.SetValues(newStock);
                }
            }
            else
            {
                await s_dbContext.AddAsync(stock);
            }
            await s_dbContext.SaveChangesAsync();

            return await GetStock(stock.StockType);
        }

        public async Task<StockDto?> GetStock(StockType stockType)
        {
            StockDto? stock =
                await s_dbContext.StockDtos.FirstOrDefaultAsync(s => s.StockType == stockType);

            return stock;
        }

        public async Task<StockDto> TakeStock(StockType stockType, int amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(amount), "Unable to take zero or negative amount.");
            }

            StockDto? stock = await GetStock(stockType);
            if (stock == null || stock.Amount < amount)
                throw new PizzaException("Not enough stock to take the given amount.");

            StockDto updatedStock = stock with { Amount = stock.Amount - amount };
            lock (s_lock)
            {
                s_dbContext.Entry(stock).CurrentValues.SetValues(updatedStock);
            }

            return stock with { Amount = amount, Id = 0 };
        }
    }
}
