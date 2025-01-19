using System.Text.RegularExpressions;

// TODO
// Combine view commands
public partial class ViewYearCommand : Command
{
    public ViewYearCommand(OverviewMenu menu) : base(menu)
    {
        PossiblePatterns.Add(SpecificYearRegex());
        PossiblePatterns.Add(CurrentYearRegex());

        CommandDescriptions = [
            ("y/year", "See all transactions for this year"),
            ("<YYYY>", "See all transactions for a specific year"),
        ];
    }

    public override void Execute(Match match, Regex pattern)
    {    
        List<Transaction> filteredTransactions;
        var menu = (OverviewMenu)Menu;

        if(pattern.Equals(CurrentYearRegex()))
        {
            filteredTransactions = menu.TService.GetTransactionsByDate(DateTime.Now.Year);
            menu.UpdateOverview(filteredTransactions);
            return;
        }

        var year = int.Parse(match.Groups[1].Value);
        filteredTransactions = menu.TService.GetTransactionsByDate(year);
        menu.UpdateOverview(filteredTransactions);
    }

    [GeneratedRegex(@"^(\d{4})$")]
    private static partial Regex SpecificYearRegex();

    [GeneratedRegex(@"^(year|y)$", RegexOptions.IgnoreCase, "en-GB")]
    private static partial Regex CurrentYearRegex();
}