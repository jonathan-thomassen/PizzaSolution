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
                        foreach (Ingredient ingredient in recipe.Ingredients)
                        {
                            Ingredient? stockDto =
                                await stockRepository.GetStock(ingredient.IngredientType);
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

        public async Task<ComparableList<Ingredient>> GetStock(
            PizzaOrder order, ComparableList<Recipe> recipeDtos)
        {
            ComparableList<Ingredient> returnList = [];

            foreach (PizzaAmount pizza in order.RequestedOrder)
            {
                foreach (Recipe recipe in recipeDtos)
                {
                    if (pizza.PizzaType == recipe.RecipeType)
                    {
                        foreach (Ingredient ingredient in recipe.Ingredients)
                        {
                            Ingredient? stockDto =
                                await stockRepository.GetStock(ingredient.IngredientType);

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
