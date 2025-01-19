using System.Text.RegularExpressions;

// TODO
// Combine view commands
public partial class ViewMonthCommand : Command
{
    OverviewMenu menu;

    public ViewMonthCommand(OverviewMenu menu) : base(menu)
    {
        this.menu = menu;
        PossiblePatterns = [
            SpecificMonthRegex(),
            CurrentMonthRegex(),
        ];

        CommandDescriptions = [
            ("m/month", "See transactions for this month"),
            ("<YYYY-MM>", "See transactions for a specific month"),
        ];
    }
    
    public override void Execute(Match match, Regex pattern)
    {
        List<Transaction> filteredTransactions;

        if(pattern.Equals(CurrentMonthRegex()))
        {
            filteredTransactions = Menu.TService.GetTransactionsByDate(DateTime.Now.Year, DateTime.Now.Month);
            menu.UpdateOverview(filteredTransactions);
            return;
        }

        var year = int.Parse(match.Groups[1].Value);
        var month = int.Parse(match.Groups[2].Value);
        filteredTransactions = Menu.TService.GetTransactionsByDate(year, month);
        menu.UpdateOverview(filteredTransactions);
    }

    [GeneratedRegex(@"^(\d{4})-(\d{2})$")]
    private static partial Regex SpecificMonthRegex();
    
    [GeneratedRegex(@"^(month|m)$", RegexOptions.IgnoreCase, "en-GB")]
    private static partial Regex CurrentMonthRegex();
}