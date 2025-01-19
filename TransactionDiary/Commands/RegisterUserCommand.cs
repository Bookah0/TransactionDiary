using System.Text.RegularExpressions;

public partial class RegisterUserCommand : Command
{
    public RegisterUserCommand(LoginMenu menu) : base (menu)
    {
        CommandDescriptions = [("register <username> <password>", "Register account")]; 
        PossiblePatterns = [RegisterRegex()];
    }

    public override void Execute(Match match, Regex pattern)
    {
        var username = match.Groups[1].Value;
        var password = match.Groups[2].Value;

        if(Menu.UService.TryFindUser(username, out var _))
        {
            Console.WriteLine("User already exists");
            return;
        }
        
        var newUser = new User
        {
            Username = username,
            Password = password,
        };

        Menu.UService.AddNewUser(newUser);
        Console.WriteLine($"User {username} created!");
    }

    [GeneratedRegex(@"^register (\S+)\s(\S+)$")]
    private static partial Regex RegisterRegex();
}