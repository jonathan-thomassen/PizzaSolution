using PizzaPlace.Models.Types;

namespace PizzaPlace.Models
{
    public record IngredientDto : IngredientBase
    {
        public long Id { get; set; }
        public IngredientDto(StockType StockType, int Amount, long id = 0) : base(StockType, Amount)
        {
            Id = id;
        }
    }
}
