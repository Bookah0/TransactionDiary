using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

// Implement argument commands into Add menu
public partial class AddCommand : Command
{
    public AddCommand(Menu menu) : base (menu)
    {
        CommandDescriptions = [
            ("<name>; <amount>", "Add an uncategorized transaction on todays date"),
            ("<name>; <amount> <category>", "Add a transaction on todays date"),
            ("<name>; <amount> <YYYY-MM-DD>", "Add an uncategorized transaction"),
            ("<name>; <amount> <YYYY-MM-DD> <category>", "Add a transaction"),
            ("a/add", "Add a transaction via the add menu"),
        ];

        PossiblePatterns = [
            AddTodayWithoutCategoryRegex(),
            AddTodayRegex(),
            AddSpecificDateNoCategoryRegex(),
            AddSpecificDateRegex(),
            AddThroughMenuRegex(),
        ];
    }

    public override void Execute(Match match, Regex pattern)
    {
        Debug.WriteLine("executing add");
        
        if(pattern == null)
        {
            Console.WriteLine("Error, requires a regex pattern to match with");
            return;
        }

        if(pattern.Equals(AddThroughMenuRegex()))
        {
            Menu.MService.SetMenu(MenuType.add);
            return;
        }

        var name = match.Groups[1].Value;
        var amount = int.Parse(match.Groups[2].Value);
        DateTime date = DateTime.UtcNow;
        //Category? category = null;

        if(DateTime.TryParseExact(match.Groups[3].Value, "dd-MM-yyyy", null, DateTimeStyles.None, out DateTime pDate))
        {
            date = pDate.ToUniversalTime();
        }
        else
        {
            /*if(!CategoryManager.TryFindCategory(match.Groups[3].Value, out category))
            {
                category = new MainCategory([], match.Groups[3].Value, "gray"); 
                CategoryManager.AddNewCategory(category);    
            }*/
        }

        var newTransaction = new Transaction
        {
            Name = name,
            Amount = amount,
            //Category = category,
            Date = date
        };

        Console.WriteLine($"New transaction: {name}, {amount}, {date}");
        Menu.TService.AddNewTransaction(newTransaction);
    }

    [GeneratedRegex(@"^(.+); (-?\d+(\.\d{1,2})?)$")]
    private static partial Regex AddTodayWithoutCategoryRegex();
    
    [GeneratedRegex(@"^(.+); (-?\d+(\.\d{1,2})?) (.+)$")]
    private static partial Regex AddTodayRegex();
    
    [GeneratedRegex(@"^(.+); (-?\d+(\.\d{1,2})?) (\d{4}-\d{2}-\d{2})$")]
    private static partial Regex AddSpecificDateNoCategoryRegex();
    
    [GeneratedRegex(@"^(.+); (-?\d+(\.\d{1,2})?) (\d{4}-\d{2}-\d{2}) (.+)$")]
    private static partial Regex AddSpecificDateRegex();

    [GeneratedRegex(@"^(add|a)$", RegexOptions.IgnoreCase, "en-GB")]
    private static partial Regex AddThroughMenuRegex();
}