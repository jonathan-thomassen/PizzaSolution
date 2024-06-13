using PizzaPlace.Models.Types;

namespace PizzaPlace.Models
{
    public record StockDto(
        StockType StockType,
        int Amount,
        long Id = 0);
}
