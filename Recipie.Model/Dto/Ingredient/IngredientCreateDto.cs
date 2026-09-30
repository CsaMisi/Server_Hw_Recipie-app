namespace Recipie.Model.Dto.Ingredient;

public class IngredientCreateDto
{
    public string Name { get; set; } = string.Empty;
    public double CaloriesPer100g { get; set; }
}
