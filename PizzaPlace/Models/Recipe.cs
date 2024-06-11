using PizzaPlace.Models.Types;

namespace PizzaPlace.Models
{
    public class Recipe
    {
        public PizzaRecipeType RecipeType { get; set; }
        public ComparableList<Ingredient> Ingredients { get; set; } = [];
        public int CookingTimeMinutes { get; set; }
        public long Id { get; set; }

        public Recipe() { }

        public Recipe(
            PizzaRecipeType recipeType,
            ComparableList<Ingredient> ingredients,
            int cookingTimeMinutes,
            long id = 0)
        {
            RecipeType = recipeType;
            Ingredients = ingredients;
            CookingTimeMinutes = cookingTimeMinutes;
            Id = id;
        }
    }
}
