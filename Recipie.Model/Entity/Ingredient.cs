namespace Recipie.Model.Entity;

public class Ingredient : IIdEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public double CaloriesPer100g { get; set; }

    public ICollection<DishIngredient> DishIngredients { get; set; } = new List<DishIngredient>();
}
