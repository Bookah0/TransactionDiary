/*public static class CategoryManager
{
    public static void AddNewCategory(Category category)
    {
        using var db = new AppContext();
        db.Categories.Add(category);
    }

    public static void RemoveCategory(Category category)
    {
        using var db = new AppContext();
        db.Categories.Remove(category);
    }

    public static List<Category> GetAllCategories()
    {
        using var db = new AppContext();
        return db.Categories.ToList();
    }

    public static Category? GetCategoryByName(string name)
    {
        using var db = new AppContext();
        return db.Categories.FirstOrDefault(t => t.Name.Equals(name));
    }

    internal static bool TryFindCategory(string value, out Category? category)
    {
        throw new NotImplementedException();
    }
}*/