// TODO 
// Move balance-methods? 

public class TransactionService
{
    UserService userService;

    public TransactionService(UserService userService)
    {
        this.userService = userService;
    }

    public void AddNewTransaction(Transaction transaction)
    {
        using var db = new AppContext();
        transaction.UserId = userService.LoggedInUser!.UserId;
        db.Transactions.Add(transaction);
        db.SaveChanges();
    }

    public void UpdateTransaction(Transaction transaction)
    {
        using var db = new AppContext();
        db.Transactions.Update(transaction);
        db.SaveChanges();
    }

    public void RemoveTransaction(Transaction transaction)
    {
        using var db = new AppContext();
        db.Transactions.Remove(transaction);
        db.SaveChanges();
    }

    public List<Transaction> GetAllTransactions()
    {
        var userId = userService.LoggedInUser!.UserId;
        
        using var db = new AppContext();
        return db.Transactions.Where(t => t.UserId == userId).ToList();
    }

    public List<Transaction> GetTransactions(string name, int? amount = null)
    {
        var userId = userService.LoggedInUser!.UserId;

        using var db = new AppContext();
        var query = db.Transactions.Where(t => t.UserId == userId && t.Name.Equals(name));

        if (amount != null)
        {
            query = query.Where(t => t.Amount == amount);
        }

        return query.ToList();
    }

    public Transaction? GetTransactionById(int id)
    {
        var userId = userService.LoggedInUser!.UserId;

        using var db = new AppContext();
        return db.Transactions.FirstOrDefault(t => t.UserId == userId && t.TransactionId == id);
    }

    public List<Transaction> GetTransactionsByDate(int year, int month = 0, int day = 0)
    {
        var userId = userService.LoggedInUser!.UserId;

        using var db = new AppContext();
        var query = db.Transactions.Where(t => t.UserId == userId && t.Date.Year == year);

        if (month != 0)
        {
            query = query.Where(t => t.Date.Month == month);
        }

        if (day != 0)
        {
            query = query.Where(t => t.Date.Day == day);
        }

        return query.ToList();
    }

    public List<Transaction> GetTransactionsByWeek(int year, int week)
    {
        var userId = userService.LoggedInUser!.UserId;

        using var db = new AppContext();
        return db.Transactions.Where(t => t.UserId == userId && t.Date.Year == year && t.GetWeekNumber() == week).ToList();
    }

    public List<Transaction>? GetTransactionsInRange(DateTime from, DateTime to)
    {
        var userId = userService.LoggedInUser!.UserId;
        from = from.ToUniversalTime();
        to = to.ToUniversalTime();

        using var db = new AppContext();
        return db.Transactions.Where(t => t.UserId == userId && t.Date >= from && t.Date <= to).ToList();
    }

    public static int GetBalance(BalanceType balanceType, List<Transaction> transactions)
    {
        var balance = 0;

        foreach (var transaction in transactions)
        {
            if(balanceType == BalanceType.Total 
                || (balanceType == BalanceType.Income && transaction.Amount > 0) 
                || (balanceType == BalanceType.Cost && transaction.Amount < 0))
            {
                balance += transaction.Amount;
            }
        }

        return balance;
    }

    /// <summary>
    /// Gets balance
    /// </summary>
    /// <param name="transactions"></param>
    /// <returns>(total, spent, income)</returns>
    public (int, int, int) GetBalance(List<Transaction> transactions)
    {
        return (GetBalance(BalanceType.Total, transactions), GetBalance(BalanceType.Cost, transactions), GetBalance(BalanceType.Income, transactions));
    }
}
