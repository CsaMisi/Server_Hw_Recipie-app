namespace Recipie.Model.Dto.Dish;

public class DishIngredientReadDto
{
    public string IngredientId { get; set; } = string.Empty;
    public string IngredientName { get; set; } = string.Empty;
    public double QuantityGrams { get; set; }
    public double CaloriesPer100g { get; set; }
    public double CaloriesContribution { get; set; }
}
