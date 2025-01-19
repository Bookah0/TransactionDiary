using System.Text.RegularExpressions;

// TODO
// Maybe implement patterns?
public static partial class RegexPatterns
{
    [GeneratedRegex(@"^(y|yes)$", RegexOptions.IgnoreCase, "en-GB")]
    public static partial Regex YesRegex();

    [GeneratedRegex(@"^(n|no)$", RegexOptions.IgnoreCase, "en-GB")]
    public static partial Regex NoRegex();
}