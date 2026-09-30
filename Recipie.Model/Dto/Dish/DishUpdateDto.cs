namespace Recipie.Model.Dto.Dish;

public class DishUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public List<DishIngredientInputDto> Ingredients { get; set; } = new();
}
