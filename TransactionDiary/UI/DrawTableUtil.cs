using Microsoft.EntityFrameworkCore.Metadata.Internal;

public enum Position
{
    Top,
    Mid,
    Bot
}

// TODO 
// Break out helpers from UITable methods and place them here 
public static class DrawTableUtil
{
    public static string CreateCorneredLine(int nColumns, int columnWidth, Position position)
    {
        (var left, var middle, var right) = GetSeperators(position);
        
        var line = left + new string('═', columnWidth);

        for (int i = 0; i < nColumns-1; i++)
        {
            line += middle + new string('═', columnWidth);
        }

        return line += right; 
    }

    public static string CreateCorneredLine(int tableWidth, Position position)
    {
        (var left, _, var right) = GetSeperators(position);
        return left + new string('═', tableWidth+1) + right;
    }

    public static (string, string, string) GetSeperators(Position position)
    {
        return position switch
        {
            Position.Top => ("╔", "╦", "╗"),
            Position.Mid => ("╠", "╬", "╣"),
            Position.Bot => ("╚", "╩", "╝"),
            _ => throw new NotImplementedException()
        };
    }
}

