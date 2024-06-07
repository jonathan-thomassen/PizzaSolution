using PizzaPlace.Models;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Repositories
{
    public interface IStockRepository
    {
        Task<Stock> AddToStock(Stock stock);
        Task<Stock?> GetStock(StockType stockType);
        Task<Stock> TakeStock(StockType stockType, int amount);
    }
}
