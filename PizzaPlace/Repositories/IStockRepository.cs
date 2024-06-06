using PizzaPlace.Models;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Repositories
{
    public interface IStockRepository
    {
        Task<StockDto> AddToStock(StockDto stock);
        Task<StockDto?> GetStock(StockType stockType);
        Task<StockDto> TakeStock(StockType stockType, int amount);
    }
}
