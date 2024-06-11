namespace PizzaPlace.Models
{
    public class RecipeIngredient()
    {
        public long RecipeId { get; set; }
        public long IngredientId { get; set; }
        public Recipe? Recipe { get; set; }
        public Ingredient? Ingredient { get; set; }
    }
}
