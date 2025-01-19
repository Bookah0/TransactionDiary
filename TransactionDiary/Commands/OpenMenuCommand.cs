using System.Text.RegularExpressions;

public partial class OpenMenuCommand : Command
{
    MenuType menuToOpen;

    public OpenMenuCommand(MenuType menuToOpen, Menu menu) : base (menu)
    {
        this.menuToOpen = menuToOpen;

        CommandDescriptions = [($"o/open {menuToOpen}", $"Open the {menuToOpen} menu")];

        string openPattern = @$"^(o|open) {menuToOpen}";
        PossiblePatterns = [new Regex(openPattern)];
    }

    public override void Execute(Match match, Regex pattern)
    {
        Menu.MService.SetMenu(menuToOpen);
    }
}