using System.Diagnostics;
using System.Text.RegularExpressions;
public enum MenuType
{
    login,
    overview,
    edit,
    add
}

// TODO 
// Redesign help-table
public abstract class Menu
{
    public List<Command> Commands { protected set; get; } = [];
    public MenuType type = 0;
    public TransactionService TService { get; private set; }
    public MenuService MService { get; private set; }
    public UserService UService { get; private set; }

    public Menu (MenuService menuService, TransactionService transactionService, UserService userService)
    {
        MService = menuService;
        TService = transactionService;
        UService = userService;

        Commands.Add(new HelpCommand(this));
        Commands.Add(new BackCommand(this));
        Commands.Add(new LogoutCommand(this));
    }
    
    public void ExecuteCommand(string input)
    {
        foreach (var command in Commands)
        {
            foreach (var pattern in command.PossiblePatterns)
            {
                if (RegexUtils.TryMatch(input, pattern, out var match))
                {
                    Debug.WriteLine($"Pattern {pattern} matched with {input}");
                    command.Execute(match, pattern);
                    return;
                }
                Debug.WriteLine($"Pattern {pattern} didnt match with {input}");
            }
        }
    }

    public abstract void Display(); 

    public UITable CreateHelpTable()
    {
        var table = new UITable(80);
        table.AddHeader(["Help"]);

        foreach (var command in Commands)
        {
            var cmds = command.CommandDescriptions.Select(c => c.Item1).ToList();
            var descriptions = command.CommandDescriptions.Select(c => c.Item2).ToList();

            table.AddContentRow([cmds, descriptions]);

            foreach (var item in cmds)
            {
                Debug.WriteLine(item);
            }

            foreach (var item in descriptions)
            {
                Debug.WriteLine(item);
            }
            
        }
        
        return table;
    }
}