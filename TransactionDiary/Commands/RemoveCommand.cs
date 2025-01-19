/*using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

public partial class RemoveCommand : Command
{

    public RemoveCommand()
    {
        CommandDescriptions = [
            ("r <name>;", "Remove a transaction with a name"),
            ("r <transaction ID>", "Remove a transaction with a specific ID"),
        ];

        PossiblePatterns = [
            RemoveByNameRegex(),
            RemoveByIdRegex(),
        ];
    }

    public override void Execute(Match match, Regex pattern)
    {
        Debug.WriteLine("executing remove");

        if(pattern.Equals(RemoveByIdRegex()))
        {
            var id = int.Parse(match.Groups[2].Value);
            var toRemove = TransactionService.GetTransactionById(id);

            if (toRemove == null)
            {
                Console.WriteLine("No transaction with that id");
                return;
            }

            ConfirmDeletion(toRemove);
            return;
        }
        
        List<Transaction> foundTransactions;
        var name = match.Groups[2].Value;
        foundTransactions = TransactionService.GetTransactions(name);

        if (foundTransactions.Count <= 0)
        {
            Console.WriteLine("Found no transaction with the given name, try again");
            return;
        }
        else if (foundTransactions.Count == 1)
        {
            ConfirmDeletion(foundTransactions[0]);
        }
        else
        {
            HandleMultiples(foundTransactions);   
        }
    }

    private void HandleMultiples(List<Transaction> foundTransactions)
    {
        Console.WriteLine("Found more than one transaction with the given data, choose which one to delete: ");

        for (int i = 0; i < foundTransactions.Count; i++)
        {
            Console.WriteLine($"{i+1}: {foundTransactions[0]}");
        }

        var answer = Console.ReadLine()!;

        if(!int.TryParse(answer, out var n) && n < 0 && n > foundTransactions.Count)
        {
            Console.WriteLine("No transaction with that number, cancelling deletion...");
            return;
        }

        ConfirmDeletion(foundTransactions[n-1]);
    }

    private void ConfirmDeletion(Transaction toDelete)
    {
        Console.WriteLine(toDelete.ToString());
        Console.WriteLine("Are you sure you want to delete this transaction? (y/n)");

        if(Console.ReadKey().Key == ConsoleKey.Y)
        {
            TransactionService.RemoveTransaction(toDelete);
            Console.WriteLine("Transaction deleted");
            return;            
        }

        Console.WriteLine("Cancelled deletion");
    }

    [GeneratedRegex(@"^(r|remove) (.+);$")]
    private static partial Regex RemoveByNameRegex();

    [GeneratedRegex(@"^(r|remove) (\d+)$")]
    private static partial Regex RemoveByIdRegex();
}*/