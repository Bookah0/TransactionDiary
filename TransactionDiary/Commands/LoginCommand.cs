using System.Text.RegularExpressions;

public partial class LoginCommand : Command
{
    public LoginCommand(LoginMenu menu) : base (menu)
    {
        CommandDescriptions = [("<username> <password>", "Login")]; 
        PossiblePatterns = [LoginRegex()];
    }

    public override void Execute(Match match, Regex pattern)
    {
        var username = match.Groups[1].Value;
        var password = match.Groups[2].Value;

        if(!Menu.UService.TryFindUser(username, out _))
        {
            Console.WriteLine("User doesn't exist");
            return;
        }

        if (!Menu.UService.TryLogin(username, password))
        {
            Console.WriteLine("Wrong password");
            return;
        }

        Menu.MService.SetMenu(MenuType.overview);
    }

    [GeneratedRegex(@"^(\S+)\s(\S+)$")]
    private static partial Regex LoginRegex();
}