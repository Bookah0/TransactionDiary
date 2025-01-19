
public class OverviewMenu : Menu
{
    public List<Transaction> visibleTransactions {get; set;}= [];

    public OverviewMenu(MenuService mService, TransactionService tService, UserService uService) : base (mService, tService, uService)
    {
        type = MenuType.overview;

        Commands.Add(new ViewYearCommand(this));
        Commands.Add(new ViewMonthCommand(this));
        Commands.Add(new ViewDayCommand(this));
        Commands.Add(new ViewWeekCommand(this));
        Commands.Add(new ViewRangeCommand(this));
        Commands.Add(new OpenMenuCommand(MenuType.edit, this));
        Commands.Add(new OpenMenuCommand(MenuType.add, this));
    }

    public override void Display()
    {
        visibleTransactions = TService.GetAllTransactions();
        var transactionTable = CreateOverviewTable();
        
        transactionTable.PrintNextTo(CreateHelpTable());
    }   

    public void UpdateOverview(List<Transaction> updatedTransactions, string? titleLine = null)
    {
        visibleTransactions = updatedTransactions;
        var transactionTable = CreateOverviewTable();
        
        if(titleLine != null)
        {
            Console.WriteLine(titleLine + "\n");
        }

        transactionTable.PrintNextTo(CreateHelpTable());
    }

    public UITable CreateOverviewTable(){ 
        visibleTransactions.Sort((x, y) => x.Date.CompareTo(y.Date));

        var incomeTransactions = visibleTransactions.Where(t => t.Amount > 0).ToList();
        var outputTransactions = visibleTransactions.Where(t => t.Amount < 0).ToList();
        (var balance, var output, var income) = TService.GetBalance(visibleTransactions);
        
        var table = new UITable(80);
        table.AddHeader(["Output:", "Income:"]);
        table.AddContentRow([outputTransactions, incomeTransactions]);
        table.AddContentRow([$"Output: {output}", $"Income: {income}"]);
        table.AddFooter([$"Balance total: {balance}"]);

        return table;
    }
}