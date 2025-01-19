// TODO
// Implement categories :((

/*public abstract class Category
{
    public int Id { get; set; }
    public string Name { get; private set;}
    // public List<Transaction> Transactions {get; private set;}
    protected string color;

    public Category(string label, string color, List<Transaction>? transactions = null)
    {
        this.Name = label;
        this.color = color;

        if(transactions == null)
        {
            Transactions = new();
        } 
        else
        {
            Transactions = transactions;
        }
    }

    public string GetDatabaseString()
    {
        return Name + ";" + ";" + color;
    }
}

public class MainCategory : Category
{
    private Dictionary<string, Category>? subcategories;

    public MainCategory(Dictionary<string, Category>? subcategories, string name, string color, List<Transaction>? transactions = null) : base(name, color, transactions)
    {
        this.subcategories = subcategories;
    }

    public Dictionary<string, Category>? GetSubategories()
    {
        return subcategories;
    }

    public void ConvertToSubcategory(Dictionary<string, Category> categoryList, MainCategory mainCategory)
    {
        if (subcategories.Count != 0)
        {
            Console.WriteLine("Must remove subcategories first");
        }
        else
        {
            var subCopy = new SubCategory(mainCategory, Name, color, Transactions);
            mainCategory.AddSubcategory(subCopy);
            categoryList[Name] = subCopy;
        }
    }

    public void AddSubcategory(SubCategory subcategory)
    {
        subcategories.Add(subcategory.Name, subcategory);
    }

    public void RemoveSubcategory(SubCategory subcategory)
    {
        subcategories.Remove(subcategory.Name);
    }
}

public class SubCategory : Category
{
    private MainCategory mainCategory;

    public SubCategory(MainCategory mainCategory, string name, string color, List<Transaction>? transactions = null) : base(name, color)
    {
        this.mainCategory = mainCategory;
    }

    public void ConvertToMainCategory(Dictionary<string, Category> categoryList)
    {
        var mainCopy = new MainCategory(new(), Name, color, Transactions);
        mainCategory.RemoveSubcategory(this);
        categoryList[Name] = mainCopy;
    }
}*/