using PizzaPlace.Models.Types;

namespace PizzaPlace.Models
{
    public class RecipeDto
    {
        public PizzaRecipeType RecipeType { get; set; }
        public ComparableList<StockDto> StockDto { get; set; } = [];
        public int CookingTimeMinutes { get; set; }
        public long Id { get; set; }

        public RecipeDto() { }

        public RecipeDto(PizzaRecipeType recipeType, ComparableList<StockDto> ingredients, int cookingTimeMinutes, long id = 0)
        {
            RecipeType = recipeType;
            StockDto = ingredients;
            CookingTimeMinutes = cookingTimeMinutes;
            Id = id;
        }
    }
}
