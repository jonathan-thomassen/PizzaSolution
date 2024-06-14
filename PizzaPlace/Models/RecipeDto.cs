using PizzaPlace.Models.Types;

namespace PizzaPlace.Models
{
    public class RecipeDto
    {
        public PizzaRecipeType RecipeType { get; set; }
        public ComparableList<IngredientDto> IngredientDtos { get; set; } = [];
        public int CookingTimeMinutes { get; set; }
        public long Id { get; set; }

        public RecipeDto() { }

        public RecipeDto(PizzaRecipeType recipeType, ComparableList<IngredientDto> ingredients, int cookingTimeMinutes, long id = 0)
        {
            RecipeType = recipeType;
            IngredientDtos = ingredients;
            CookingTimeMinutes = cookingTimeMinutes;
            Id = id;
        }
    }
}
