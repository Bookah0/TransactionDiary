using System.Text.RegularExpressions;

public partial class LogoutCommand : Command
{
    public LogoutCommand(Menu menu) : base (menu)
    {
        CommandDescriptions = [("logout", "Logout and go to login menu")]; 
        PossiblePatterns = [LogoutRegex()];
    }

    public override void Execute(Match match, Regex pattern)
    {
        Console.WriteLine("Are you sure you want to logout? (y/n)");

        if(Console.ReadKey().Key == ConsoleKey.N)
        {
            Console.WriteLine("Canceling logout");
            ConsoleUtil.PrintDots(100, 3);
            Menu.MService.currentMenu.Display();
        }
        
        Menu.UService.Logout();
        Menu.MService.SetMenu(MenuType.login);
    }

    [GeneratedRegex(@"^(logout)$", RegexOptions.IgnoreCase, "en-GB")]
    private static partial Regex LogoutRegex();
}