namespace Recipie.Model.Dto.Dish;

public class DishReadDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public double TotalCalories { get; set; }
    public List<DishIngredientReadDto> Ingredients { get; set; } = new();
}
