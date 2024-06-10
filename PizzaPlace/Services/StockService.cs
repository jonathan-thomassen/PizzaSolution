using PizzaPlace.Models;
using PizzaPlace.Repositories;

namespace PizzaPlace.Services
{
    public class StockService(IStockRepository stockRepository) : IStockService
    {
        public async Task<bool> HasInsufficientStock(
            PizzaOrder order, ComparableList<Recipe> recipeDtos)
        {
            foreach (PizzaAmount pizza in order.RequestedOrder)
            {
                foreach (Recipe recipe in recipeDtos)
                {
                    if (pizza.PizzaType == recipe.RecipeType)
                    {
                        foreach (Stock ingredient in recipe.Stock)
                        {
                            Stock? stockDto =
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

        public async Task<ComparableList<Stock>> GetStock(
            PizzaOrder order, ComparableList<Recipe> recipeDtos)
        {
            ComparableList<Stock> returnList = [];

            foreach (PizzaAmount pizza in order.RequestedOrder)
            {
                foreach (Recipe recipe in recipeDtos)
                {
                    if (pizza.PizzaType == recipe.RecipeType)
                    {
                        foreach (Stock ingredient in recipe.Stock)
                        {
                            Stock? stockDto =
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
