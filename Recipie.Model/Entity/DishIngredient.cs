namespace Recipie.Model.Entity;

public class DishIngredient
{
    public string DishId { get; set; } = string.Empty;
    public string IngredientId { get; set; } = string.Empty;
    public double QuantityGrams { get; set; }

    public Dish Dish { get; set; } = null!;
    public Ingredient Ingredient { get; set; } = null!;
}
