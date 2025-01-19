using System.Text.RegularExpressions;

public partial class BackCommand : Command
{
    public BackCommand(Menu menu) : base (menu)
    {
        CommandDescriptions = [("b/back", "Go back to the previous menu")];
        PossiblePatterns = [BackRegex()];
    }

    public override void Execute(Match match, Regex pattern)
    {
        Menu.MService.SetMenuToPrevious();
    }

    [GeneratedRegex(@"^(b|back)$", RegexOptions.IgnoreCase, "en-GB")]
    private static partial Regex BackRegex();
}