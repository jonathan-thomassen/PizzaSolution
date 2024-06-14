using PizzaPlace.Models;

namespace PizzaPlace.Extensions
{
    public static class PizzaHelperExtensions
    {
        public static IEnumerable<IngredientBase> GetRequiredStock(
            this IEnumerable<PizzaPrepareOrder> prepareOrders) => prepareOrders
            .SelectMany(order => Enumerable.Range(0, order.OrderAmount)
            .Select(_ => order.RecipeDto))
            .SelectMany(recipe => recipe.IngredientDtos)
            .GatherSameTypeOfStock();

        public static IEnumerable<IngredientBase> GatherSameTypeOfStock(
            this IEnumerable<IngredientBase> stock) => stock
            .GroupBy(stock => stock.StockType)
            .Select(stockGroup => new StockDto(stockGroup.Key, stockGroup.Sum(x => x.Amount)));

        public static bool HasEnough(
            this IEnumerable<IngredientBase> availableStock, IEnumerable<IngredientBase> requiredStock) =>
            requiredStock.GatherSameTypeOfStock()
            .All(required => availableStock.Any(
                x => x.StockType == required.StockType && x.Amount >= required.Amount));
    }
}
