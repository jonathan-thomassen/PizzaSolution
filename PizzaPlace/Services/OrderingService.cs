using PizzaPlace.Factories;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Pizzas;

namespace PizzaPlace.Services
{
    public class OrderingService(
        IStockService stockService,
        IRecipeService recipeService,
        IPizzaOven pizzaOven)
        : IOrderingService
    {
        public async Task<IEnumerable<Pizza>> HandlePizzaOrder(PizzaOrder order)
        {
            ComparableList<PizzaRecipe> recipes = await recipeService.GetPizzaRecipes(order);
            if (await stockService.HasInsufficientStock(order, recipes))
                throw new PizzaException("Unable to take in order. Insufficient stock.");

            ComparableList<Stock> stock =  await stockService.GetStock(order, recipes);

            ComparableList<PizzaPrepareOrder> prepareOrder = order.RequestedOrder
                .GroupBy(x => x.PizzaType)
                .Select(x => new PizzaPrepareOrder(GetPizzaRecipe(x.Key, recipes)
                , x.Aggregate(0, (total, request) => total + request.Amount)))
                .ToComparableList();

            return await pizzaOven.PreparePizzas(prepareOrder, stock);

            PizzaRecipe GetPizzaRecipe(
                PizzaRecipeType pizzaType, ComparableList<PizzaRecipe> recipes) =>
                recipes.FirstOrDefault(x => x.RecipeType == pizzaType) ??
                throw new PizzaException("Missing recipe. Recipe service did not return a " +
                                         $"recipe for {pizzaType} which was expected.");
        }
    }
}
