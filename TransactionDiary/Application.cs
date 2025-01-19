using Microsoft.EntityFrameworkCore;

// TODO 
// DI for services
public class Application
{
    static void Main(string[] args)
    {
        using var context = new AppContext();
        context.Database.Migrate();

        var userService = new UserService();
        var transactionService = new TransactionService(userService);
        var menuService = new MenuService(transactionService, userService);
        
        while(true)
        {
            string input = Console.ReadLine()!;
            menuService.currentMenu.ExecuteCommand(input);
        }
    }
}