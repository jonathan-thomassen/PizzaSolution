using Microsoft.VisualStudio.TestTools.UnitTesting;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;

namespace PizzaPlace.Test.Repositories;

[TestClass]
public class RecipeRepositoryTests
{
    private static RecipeRepository GetRecipeRepository() => new();

    private static ComparableList<Ingredient> GetStandardIngredients() =>
    [
        new Ingredient(IngredientType.Dough, 3),
        new Ingredient(IngredientType.Tomatoes, 10),
        new Ingredient(IngredientType.Bacon, 2),
    ];
    private const int StandardCookingTime = 19;

    [TestMethod]
    public async Task AddRecipe()
    {
        // Arrange
        Recipe recipe = new(
            PizzaRecipeType.StandardPizza,
            GetStandardIngredients(),
            StandardCookingTime);
        RecipeRepository repository = GetRecipeRepository();

        // Act
        long actual = await repository.AddRecipe(recipe);

        // Assert
        Assert.IsTrue(actual > 0, "Recipe has an id.");

        // Cleanup
        await repository.DeleteRecipe(recipe.Id);
    }

    [TestMethod]
    public async Task AddRecipe_AlreadyAdded()
    {
        // Arrange
        Recipe oldRecipe = new(
            PizzaRecipeType.StandardPizza,
            GetStandardIngredients(),
            StandardCookingTime);

        RecipeRepository repository = GetRecipeRepository();
        await repository.AddRecipe(oldRecipe);

        Recipe newRecipe = new(
            PizzaRecipeType.StandardPizza,
            [new(IngredientType.UnicornDust, 123), new(IngredientType.Anchovies, 1)],
            StandardCookingTime,
            oldRecipe.Id);

        // Act
        InvalidOperationException ex =
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
            repository.AddRecipe(newRecipe));

        // Assert
        Assert.AreEqual(
            "The instance of entity type 'Recipe' cannot be tracked because another instance " +
            "with the same key value for {'Id'} is already being tracked. When attaching " +
            "existing entities, ensure that only one entity instance with a given key value is " +
            "attached. Consider using 'DbContextOptionsBuilder.EnableSensitiveDataLogging' to " +
            "see the conflicting key values.", ex.Message);

        // Cleanup        
        await repository.DeleteRecipe(oldRecipe.Id);
    }

    [TestMethod]
    public async Task GetRecipe()
    {
        // Arrange
        PizzaRecipeType pizzaType = PizzaRecipeType.StandardPizza;

        Recipe recipe = new(
            PizzaRecipeType.StandardPizza,
            GetStandardIngredients(),
            StandardCookingTime);
        RecipeRepository repository = GetRecipeRepository();

        await repository.AddRecipe(recipe);

        // Act
        Recipe? actual = await repository.GetRecipe(pizzaType);

        // Assert
        Assert.AreEqual(recipe, actual);

        // Cleanup
        await repository.DeleteRecipe(recipe.Id);
    }

    [TestMethod]
    public async Task GetRecipe_DoesNotExist()
    {
        // Arrange
        PizzaRecipeType pizzaType = PizzaRecipeType.ExtremelyTastyPizza;
        RecipeRepository repository = GetRecipeRepository();

        // Act
        PizzaException ex = await Assert.ThrowsExceptionAsync<PizzaException>(() => repository.GetRecipe(pizzaType));

        // Assert
        Assert.AreEqual("Recipe does not exists of type ExtremelyTastyPizza.", ex.Message);
    }
}
