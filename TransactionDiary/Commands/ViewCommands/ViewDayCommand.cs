using System.Text.RegularExpressions;

// TODO
// Combine view commands
public partial class ViewDayCommand : Command
{
    public ViewDayCommand(OverviewMenu menu) : base(menu)
    {
        PossiblePatterns.Add(SpecificDayRegex());
        PossiblePatterns.Add(TodayRegex());

        CommandDescriptions = [
            ("d/day", "See transactions for today"),
            ("<YYYY-MM-DD>", "See all transactions for a specific day"),
        ];
    }
    
    public override void Execute(Match match, Regex pattern)
    {
        List<Transaction> filteredTransactions;
        var menu = (OverviewMenu)Menu;

        if (pattern.Equals(TodayRegex()))
        {
            filteredTransactions = Menu.TService.GetTransactionsByDate(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            menu.UpdateOverview(filteredTransactions);
            return;
        }

        var year = int.Parse(match.Groups[1].Value);
        var month = int.Parse(match.Groups[2].Value);
        var day = int.Parse(match.Groups[3].Value);
        filteredTransactions = Menu.TService.GetTransactionsByDate(year, month, day);
        menu.UpdateOverview(filteredTransactions);
    }

    [GeneratedRegex(@"^(day|d)$", RegexOptions.IgnoreCase, "en-GB")]
    private static partial Regex TodayRegex();

    [GeneratedRegex(@"^(\d{4})-(\d{2})-(\d{2})$")]
    private static partial Regex SpecificDayRegex();
}