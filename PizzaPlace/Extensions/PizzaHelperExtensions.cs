using PizzaPlace.Models;

namespace PizzaPlace.Extensions
{
    public static class PizzaHelperExtensions
    {
        public static IEnumerable<Ingredient> GetRequiredStock(
            this IEnumerable<PizzaPrepareOrder> prepareOrders) => prepareOrders
            .SelectMany(order => Enumerable.Range(0, order.OrderAmount)
            .Select(_ => order.RecipeDto))
            .SelectMany(recipe => recipe.Stock)
            .GatherSameTypeOfStock();

        public static IEnumerable<Ingredient> GatherSameTypeOfStock(
            this IEnumerable<Ingredient> stock) => stock
            .GroupBy(ingredient => ingredient.IngredientType)
            .Select(stockGroup => new Ingredient(stockGroup.Key, stockGroup.Sum(x => x.Amount)));

        public static bool HasEnough(
            this IEnumerable<Ingredient> availableStock, IEnumerable<Ingredient> requiredStock) =>
            requiredStock.GatherSameTypeOfStock()
            .All(required => availableStock.Any(
                x => x.IngredientType == required.IngredientType && x.Amount >= required.Amount));
    }
}
