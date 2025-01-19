using System.Globalization;
using System.Text.RegularExpressions;

// TODO
// Combine view commands
public partial class ViewWeekCommand : Command
{
    public ViewWeekCommand(OverviewMenu menu) : base(menu)
    {
        PossiblePatterns.Add(SpecificWeekRegex());
        PossiblePatterns.Add(CurrentWeekRegex()); 

        CommandDescriptions = [
            ("w/week", "See transactions for this week"),
            ("<YYYY> <Weeknumber>", "See all transactions for a specific week"),
        ];
    }
    
    public override void Execute(Match match, Regex pattern)
    {
        List<Transaction> filteredTransactions;
        var menu = (OverviewMenu)Menu;

        if(pattern.Equals(CurrentWeekRegex()))
        {
            var weekNumber = DateUtils.GetCurrentWeek();
            filteredTransactions = Menu.TService.GetTransactionsByWeek(DateTime.Now.Year, weekNumber);
            menu.UpdateOverview(filteredTransactions);
            return;
        }

        var year = int.Parse(match.Groups[1].Value);
        var week = int.Parse(match.Groups[2].Value);
        filteredTransactions = Menu.TService.GetTransactionsByDate(year, week);
        menu.UpdateOverview(filteredTransactions);
    }

    [GeneratedRegex(@"^(week|w)$", RegexOptions.IgnoreCase, "en-GB")]
    private static partial Regex CurrentWeekRegex();

    [GeneratedRegex(@"^(\d{4})\s(\d{1,2})$")]
    private static partial Regex SpecificWeekRegex();
}