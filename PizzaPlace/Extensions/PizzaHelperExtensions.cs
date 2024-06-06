using PizzaPlace.Models;

namespace PizzaPlace.Extensions
{
    public static class PizzaHelperExtensions
    {
        public static IEnumerable<StockDto> GetRequiredStock(
            this IEnumerable<PizzaPrepareOrder> prepareOrders) => prepareOrders
            .SelectMany(order => Enumerable.Range(0, order.OrderAmount)
            .Select(_ => order.RecipeDto))
            .SelectMany(recipe => recipe.Ingredients)
            .GatherSameTypeOfStock();

        public static IEnumerable<StockDto> GatherSameTypeOfStock(
            this IEnumerable<StockDto> stock) => stock
            .GroupBy(stock => stock.StockType)
            .Select(stockGroup => new StockDto(stockGroup.Key, stockGroup.Sum(x => x.Amount)));

        public static bool HasEnough(
            this IEnumerable<StockDto> availableStock, IEnumerable<StockDto> requiredStock) =>
            requiredStock.GatherSameTypeOfStock()
            .All(required => availableStock.Any(
                x => x.StockType == required.StockType && x.Amount >= required.Amount));
    }
}
