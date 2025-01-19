public class LoginMenu : Menu
{
    public LoginMenu(MenuService mService, TransactionService tService, UserService uService) : base (mService, tService, uService)
    {
        type = MenuType.login;

        Commands.Add(new LoginCommand(this));
        Commands.Add(new LogoutCommand(this));
        Commands.Add(new RegisterUserCommand(this));
    }

    public override void Display()
    {
        CreateHelpTable().PrintTable();
        Console.WriteLine("Welcome, please login");
    }
}