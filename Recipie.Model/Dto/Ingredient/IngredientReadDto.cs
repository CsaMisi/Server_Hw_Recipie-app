namespace Recipie.Model.Dto.Ingredient;

public class IngredientReadDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public double CaloriesPer100g { get; set; }
}
