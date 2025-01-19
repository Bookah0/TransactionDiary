using System;
using System.Text.RegularExpressions;

public abstract class Command
{
    protected Menu Menu { get; private set; }

    public Command(Menu menu)
    {
        Menu = menu;
    }
    
    public (string, string)[] CommandDescriptions {get; set;} = [];

    public List<Regex> PossiblePatterns {get; set;} = [];

    public abstract void Execute(Match match, Regex pattern);
  
}