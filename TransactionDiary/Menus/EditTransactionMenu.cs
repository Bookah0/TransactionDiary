using Microsoft.EntityFrameworkCore.Metadata.Internal;

// TODO
// Make highlighting work as expected!
public class EditTransactionMenu : Menu
{
    List<List<Transaction>> pages = [];
    List<Transaction> transactions = [];
    int itemsOnPage = 30;
    int curPageInd = 0;
    int hoveredTransactionInd = 0;

    public EditTransactionMenu(MenuService mService, TransactionService tService, UserService uService) : base (mService, tService, uService)
    {
        type = MenuType.edit;
        Commands.Add(new OpenMenuCommand(MenuType.overview, this));
    }

    public override void Display()
    {
        transactions = TService.GetAllTransactions();
        transactions.Sort((x, y) => x.Date.CompareTo(y.Date));
        pages = TransactionUtility.SplitIntoPages(transactions, itemsOnPage);
        Console.WriteLine(pages[0].Count);

        var transactionTable = CreateTransactionTable(pages[curPageInd]);
        //var helpTable = CreateHelpTable();

        transactionTable.PrintTable();
        //transactionTable.PrintNextTo(helpTable);

        HandleChooseTransaction();
    }

    public UITable CreateTransactionTable(List<Transaction> page)
    {
        var ids = page.Select(t => t.TransactionId.ToString()).ToList();
        var names = page.Select(t => t.Name).ToList();
        var amounts = page.Select(t => t.Amount.ToString()).ToList();
        var dates = page.Select(t => t.Date.ToString()).ToList();

        var table = new UITable(100);
        table.AddHeader(["ID:", "Name:", "Amount:", "Date:"]);
        table.AddContentRow([ids, names, amounts, dates]);

        return table;
    }

    public void HandleChooseTransaction()
    {
        Console.WriteLine("Write a transaction ID. b/back to go back");

        while (true)
        {
            var input = Console.ReadLine()!;

            if(input.Equals("b", StringComparison.CurrentCultureIgnoreCase) || input.Equals("back", StringComparison.CurrentCultureIgnoreCase))
            {
                MService.SetMenuToPrevious();
                return;
            }

            if(!int.TryParse(input, out var id))
            {
                Console.WriteLine("Write a number");
                continue;
            }

            var toEdit = TService.GetTransactionById(id);

            if(toEdit == null)
            {
                Console.WriteLine("No transaction with that ID");
                continue;
            }

            HandleEditTransaction(toEdit);
        }
    }

    public void HandleEditTransaction(Transaction toEdit)
    {
        ConsoleKeyInfo keyInfo;
        PrintEditScreen(toEdit);

        while (true)
        {
            while (!Console.KeyAvailable)
            {
                Thread.Sleep(250);
            }

            keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.D1:
                    HandleChangeName(toEdit);
                    break;

                case ConsoleKey.D2:
                    HandleChangeAmount(toEdit);
                    break;

                case ConsoleKey.D3:
                    HandleChangeDate(toEdit);
                    break;

                case ConsoleKey.D4:
                    HandleRemove(toEdit);
                    break;

                case ConsoleKey.Escape or ConsoleKey.B:
                    Display();
                    return;
            }
        }
    }

    private void PrintEditScreen(Transaction toEdit)
    {
        var transactionTable = CreateTransactionTable([toEdit]);
        transactionTable.PrintTable();
        
        Console.WriteLine("esc/b. Back");
        Console.WriteLine("1. Edit name");
        Console.WriteLine("2. Edit amount");
        Console.WriteLine("3. Edit date");
        Console.WriteLine("4. Remove");
    }

    public void HandleChangeName(Transaction toEdit)
    {
        Console.WriteLine("Write a new name");
        var newName = Console.ReadLine()!;
        toEdit.Name = newName;
        TService.UpdateTransaction(toEdit);
        PrintEditScreen(toEdit);
    }

    public void HandleChangeAmount(Transaction toEdit)
    {
        Console.WriteLine("Write a new amount");
        var newAmount = Console.ReadLine()!;

        if (!int.TryParse(newAmount, out var amount))
        {
            Console.WriteLine("Bad input, not a number");
            return;
        }

        toEdit.Amount = amount;
        TService.UpdateTransaction(toEdit);
        PrintEditScreen(toEdit);
    }

    public void HandleChangeDate(Transaction toEdit)
    {
        Console.WriteLine("Write a new date");
        var newDate = Console.ReadLine()!;

        if (!DateTime.TryParse(newDate, out var date))
        {
            Console.WriteLine("Bad input, not a date");
            return;
        }

        toEdit.Date = date;
        TService.UpdateTransaction(toEdit);
        PrintEditScreen(toEdit);
    }

    public void HandleRemove(Transaction toEdit)
    {
        Console.WriteLine($"Are you sure you vant to delete {toEdit.Name}? (y/n)");
        var confirmation = Console.ReadLine()!;

        if (!confirmation.Equals("y", StringComparison.CurrentCultureIgnoreCase))
        {
            Console.WriteLine("Cancelled deletion");
            return;
        }
        TService.RemoveTransaction(toEdit);
        Display();
    }

    public void NextPage()
    {
        if (curPageInd < pages.Count - 1)
        {
            curPageInd += 1;
            Display();
        }
    }

    public void PreviousPage()
    {
        if (curPageInd > 0)
        {
            curPageInd -= 1;
            Display();
        }
    }

    public void HoverAbove()
    {
        if (hoveredTransactionInd > 0)
        {
            hoveredTransactionInd -= 1;
            Display();
        }
    }

    public void HoverBelow()
    {
        if (hoveredTransactionInd < pages[curPageInd].Count - 1)
        {
            hoveredTransactionInd += 1;
            Display();
        }
    }
}

/*
HIGHLIGHT CODE

    public void HandleChooseTransaction()
    {
        ConsoleKeyInfo keyInfo;
        Console.WriteLine("\nPress a key to display; press the 'x' key to quit.");

        while (true)
        {
            while (!Console.KeyAvailable)
                Thread.Sleep(250);

            keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow or ConsoleKey.W:
                    HoverAbove();
                    break;

                case ConsoleKey.DownArrow or ConsoleKey.S:
                    HoverBelow();
                    break;

                case ConsoleKey.LeftArrow or ConsoleKey.A:
                    PreviousPage();
                    break;

                case ConsoleKey.RightArrow or ConsoleKey.D:
                    NextPage();
                    break;

                case ConsoleKey.Enter:
                    HandleEditTransaction(pages[curPageInd][hoveredTransactionInd]);
                    break;

                case ConsoleKey.Escape:
                    MService.SetMenuToPrevious();
                    return;
            }
        }
    }

    public void HandleEditTransaction(Transaction toEdit)
    {
        ConsoleKeyInfo keyInfo;
        PrintEditScreen(toEdit);

        while (true)
        {
            while (!Console.KeyAvailable)
            {
                Thread.Sleep(250);
            }

            keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.D1:
                    HandleChangeName(toEdit);
                    return;

                case ConsoleKey.D2:
                    HandleChangeAmount(toEdit);
                    break;

                case ConsoleKey.D3:
                    HandleChangeDate(toEdit);
                    break;

                case ConsoleKey.D4:
                    HandleRemove(toEdit);
                    break;

                case ConsoleKey.Escape or ConsoleKey.B:
                    Display();
                    return;
            }
        }
    }

    private void PrintEditScreen(Transaction toEdit)
    {
        var transactionTable = CreateTransactionTable([toEdit]);
        transactionTable.PrintTable();
        
        Console.WriteLine("esc/b. Back");
        Console.WriteLine("1. Edit name");
        Console.WriteLine("2. Edit amount");
        Console.WriteLine("3. Edit date");
        Console.WriteLine("4. Remove");
    }

    public void HandleChangeName(Transaction toEdit)
    {
        Console.WriteLine("Write a new name");
        var newName = Console.ReadLine()!;
        toEdit.Name = newName;
        TService.UpdateTransaction(toEdit);
        PrintEditScreen(toEdit);
    }

    public void HandleChangeAmount(Transaction toEdit)
    {
        Console.WriteLine("Write a new amount");
        var newAmount = Console.ReadLine()!;

        if (!int.TryParse(newAmount, out var amount))
        {
            Console.WriteLine("Bad input, not a number");
            return;
        }

        toEdit.Amount = amount;
        TService.UpdateTransaction(toEdit);
        PrintEditScreen(toEdit);
    }

    public void HandleChangeDate(Transaction toEdit)
    {
        Console.WriteLine("Write a new date");
        var newDate = Console.ReadLine()!;

        if (!DateTime.TryParse(newDate, out var date))
        {
            Console.WriteLine("Bad input, not a date");
            return;
        }

        toEdit.Date = date;
        TService.UpdateTransaction(toEdit);
        PrintEditScreen(toEdit);
    }

    public void HandleRemove(Transaction toEdit)
    {
        Console.WriteLine($"Are you sure you vant to delete {toEdit.Name}? (y/n)");
        var confirmation = Console.ReadLine()!;

        if (!confirmation.Equals("y", StringComparison.CurrentCultureIgnoreCase))
        {
            Console.WriteLine("Cancelled deletion");
            return;
        }
        TService.RemoveTransaction(toEdit);
        Display();
    }

    public void NextPage()
    {
        if (curPageInd < pages.Count - 1)
        {
            curPageInd += 1;
            Display();
        }
    }

    public void PreviousPage()
    {
        if (curPageInd > 0)
        {
            curPageInd -= 1;
            Display();
        }
    }

    public void HoverAbove()
    {
        if (hoveredTransactionInd > 0)
        {
            hoveredTransactionInd -= 1;
            Display();
        }
    }

    public void HoverBelow()
    {
        if (hoveredTransactionInd < pages[curPageInd].Count - 1)
        {
            hoveredTransactionInd += 1;
            Display();
        }
    }
*/