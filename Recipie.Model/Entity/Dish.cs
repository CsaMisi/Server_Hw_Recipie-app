namespace Recipie.Model.Entity;

public class Dish : IIdEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;

    public ICollection<DishIngredient> DishIngredients { get; set; } = new List<DishIngredient>();
}
