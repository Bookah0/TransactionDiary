using System.Text.RegularExpressions;

public static class RegexUtils
{
    public static bool TryMatch(string input, Regex regex, out Match match)
    {
        match = regex.Match(input);
        return match.Success;
    }
}