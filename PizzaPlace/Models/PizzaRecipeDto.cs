using PizzaPlace.Models.Types;

namespace PizzaPlace.Models;

public record PizzaRecipeDto(PizzaRecipeType RecipeType, ComparableList<StockDto> Ingredients, int CookingTimeMinutes, long Id = 0) : Dto(Id);
