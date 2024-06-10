namespace PizzaPlace.Models
{
    public class RecipeStock()
    {
        public long RecipeId { get; set; }
        public long StockId { get; set; }
        public Recipe? Recipe { get; set; }
        public Stock? Stock { get; set; }
    }
}
