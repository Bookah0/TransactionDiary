using System.Text.RegularExpressions;

// TODO
// Combine view commands
public partial class ViewRangeCommand : Command
{
    public ViewRangeCommand(OverviewMenu menu) : base(menu)
    {
        PossiblePatterns.Add(SpecificRangeRegex());
        PossiblePatterns.Add(AllRegex());

        CommandDescriptions = [
            ("a/all", "See all transactions"),
            ("<YYYY-MM-DD> <YYYY-MM-DD>", "See all transactions betwwen two dates"),
        ];
    }

    public override void Execute(Match match, Regex pattern)
    {
        List<Transaction>? filteredTransactions;

        if (pattern.Equals(AllRegex()))
        {
            filteredTransactions = Menu.TService.GetTransactionsInRange(new DateTime(1000, 1, 1), DateTime.Now);
        }
        else
        {
            var year1 = int.Parse(match.Groups[1].Value);
            var month1 = int.Parse(match.Groups[2].Value);
            var day1 = int.Parse(match.Groups[3].Value);

            var year2 = int.Parse(match.Groups[4].Value);
            var month2 = int.Parse(match.Groups[5].Value);
            var day2 = int.Parse(match.Groups[6].Value);

            var fromDate = new DateTime(year1, month1, day1);
            var toDate = new DateTime(year2, month2, day2);

            filteredTransactions = Menu.TService.GetTransactionsInRange(fromDate, toDate);
        }

        if (filteredTransactions != null)
        {
            var menu = (OverviewMenu)Menu;
            menu.UpdateOverview(filteredTransactions);
        }
    }

    [GeneratedRegex(@"^<(\d{4})-(\d{2})-(\d{2})>\s<(\d{4})-(\d{2})-(\d{2})>$")]
    private static partial Regex SpecificRangeRegex();

    [GeneratedRegex(@"^(all|a)$", RegexOptions.IgnoreCase, "en-GB")]
    private static partial Regex AllRegex();
}