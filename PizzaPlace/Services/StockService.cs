using PizzaPlace.Models;
using PizzaPlace.Repositories;

namespace PizzaPlace.Services
{
    public class StockService(IStockRepository stockRepository) : IStockService
    {
        public async Task<bool> HasInsufficientStock(
            PizzaOrder order, ComparableList<RecipeDto> recipeDtos)
        {
            foreach (PizzaAmount pizza in order.RequestedOrder)
            {
                foreach (RecipeDto recipe in recipeDtos)
                {
                    if (pizza.PizzaType == recipe.RecipeType)
                    {
                        foreach (StockDto ingredient in recipe.StockDto)
                        {
                            StockDto? stockDto =
                                await stockRepository.GetStock(ingredient.StockType);
                            if (stockDto == null || stockDto.Amount < ingredient.Amount)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public async Task<ComparableList<StockDto>> GetStock(
            PizzaOrder order, ComparableList<RecipeDto> recipeDtos)
        {
            ComparableList<StockDto> returnList = [];

            foreach (PizzaAmount pizza in order.RequestedOrder)
            {
                foreach (RecipeDto recipe in recipeDtos)
                {
                    if (pizza.PizzaType == recipe.RecipeType)
                    {
                        foreach (StockDto ingredient in recipe.StockDto)
                        {
                            StockDto? stockDto =
                                await stockRepository.GetStock(ingredient.StockType);

                            if (stockDto != null)
                            {
                                returnList.Add(stockDto);
                            }
                        }
                    }
                }
            }

            return returnList;
        }
    }
}
