using System.Text.RegularExpressions;

public partial class HelpCommand : Command
{
    public HelpCommand(Menu menu) : base (menu)
    {
        CommandDescriptions = [("h/help", "View all commands for this menu")];
        PossiblePatterns = [HelpRegex()];
    }

    public override void Execute(Match match, Regex pattern)
    {
        var helpTable = Menu.MService.currentMenu.CreateHelpTable();
        helpTable.PrintTable();
    }

    [GeneratedRegex(@"^(h|help)$", RegexOptions.IgnoreCase, "en-GB")]
    private static partial Regex HelpRegex();
}