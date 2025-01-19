using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class AddTransactionMenu : Menu
{
    Transaction newTransaction = new();

    public AddTransactionMenu(MenuService mService, TransactionService tService, UserService uService) : base (mService, tService, uService)
    {
        type = MenuType.edit;
    }

    public override void Display()
    {
        newTransaction = new Transaction
        {
            Name = "",
            Amount = 0,
            Date = DateTime.UtcNow
        };
        
        HandleEditTransaction();
        MService.SetMenu(MenuType.overview);
    }

    public UITable CreateTransactionTable()
    {
        var name = newTransaction.Name;
        var amount = newTransaction.Amount.ToString();
        var date = newTransaction.Date.ToString();

        var table = new UITable(100);
        table.AddHeader(["Name:", "Amount:", "Date:"]);
        table.AddContentRow([[name], [amount], [date]]);

        return table;
    }

    public void HandleEditTransaction()
    {
        ConsoleKeyInfo keyInfo;
        PrintEditScreen();

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
                    HandleChangeName();
                    break;

                case ConsoleKey.D2:
                    HandleChangeAmount();
                    break;

                case ConsoleKey.D3:
                    HandleChangeDate();
                    break;

                case ConsoleKey.D4:
                    if (HandleSave()) return;
                    break;

                case ConsoleKey.Escape or ConsoleKey.B:
                    MService.SetMenu(MenuType.overview);
                    return;
            }
        }
    }

    private void PrintEditScreen()
    {
        var transactionTable = CreateTransactionTable();
        transactionTable.PrintTable();
        
        Console.WriteLine("esc/b. Back");
        Console.WriteLine("1. Edit name");
        Console.WriteLine("2. Edit amount");
        Console.WriteLine("3. Edit date");
        Console.WriteLine("4. Save");
    }

    public void HandleChangeName()
    {
        Console.WriteLine("Write a new name");
        var newName = Console.ReadLine()!;
        newTransaction.Name = newName;
        TService.UpdateTransaction(newTransaction);
        PrintEditScreen();
    }

    public void HandleChangeAmount()
    {
        Console.WriteLine("Write a new amount");
        var newAmount = Console.ReadLine()!;

        if (!int.TryParse(newAmount, out var amount))
        {
            Console.WriteLine("Bad input, not a number");
            return;
        }

        newTransaction.Amount = amount;
        TService.UpdateTransaction(newTransaction);
        PrintEditScreen();
    }

    public void HandleChangeDate()
    {
        Console.WriteLine("Write a new date");
        var newDate = Console.ReadLine()!;

        if (!DateTime.TryParse(newDate, out var date))
        {
            Console.WriteLine("Bad input, not a date");
            return;
        }

        newTransaction.Date = date;
        PrintEditScreen();
    }

    public bool HandleSave()
    {
        if(string.IsNullOrEmpty(newTransaction.Name))
        {
            Console.WriteLine("You must add a name.");
            return false;
        }
        if(newTransaction.Amount == 0)
        {
            Console.WriteLine("You must add an amount.");
            return false;
        }

        Console.WriteLine($"Are you sure you want to save {newTransaction.Name}? (y/n)");
        var confirmation = Console.ReadLine()!;

        if (!confirmation.Equals("y", StringComparison.CurrentCultureIgnoreCase))
        {
            Console.WriteLine("Transaction wans't saved");
            return false;
        }

        TService.AddNewTransaction(newTransaction);
        return true;
    }
}