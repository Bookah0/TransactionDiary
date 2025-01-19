using System.Diagnostics;
using Azure.Core.GeoJson;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

// TODO 
// Dynamic spacing/padding
// Dynamic highlighting
// Replace PrintNextTo() into "Combine tables?"
public class UITable
{
    List<string> header = [];
    List<string> contentRows = [];
    List<string> footer = [];
    int tableWidth;
    (int, int) highLightStart;
    (int, int) highLightEnd;

    public UITable(int tableWidth = 80)
    {
        this.tableWidth = tableWidth;
    }

    public UITable(List<string> header, List<string> contentRows, List<string> footer)
    {
        this.header = header;
        this.contentRows = contentRows;
        this.footer = footer;
    }

    public void AddContentRow<T>(List<List<T>> contents, bool dynamicSpacing = false) where T : class
    {
        var nColumns = contents.Count;
        var columnWidth = tableWidth/nColumns;
        var maxRows = contents.Max(c => c.Count);
        int[] columnSpacing = GetColumnSpacing(contents, columnWidth, dynamicSpacing);

        for (int r = 0; r < maxRows; r++)
        {
            var contentLine = "║";

            for (int c = 0; c < contents.Count; c++)
            {   
                var padding = int.Max(0, columnSpacing[c] - 2);
                Debug.WriteLine($"Padding: {padding}");

                if(r < contents[c].Count)
                {
                    contentLine += $" {contents[c][r]!.ToString()!.PadRight(padding)} ║";
                }
                else
                {
                    contentLine += $" {" ".PadRight(padding)} ║";
                }
            }

            contentRows.Add(contentLine);
        }

        contentRows.Add(DrawTableUtil.CreateCorneredLine(nColumns, columnWidth, Position.Mid)); 
    }

    /* 
    Highlight code
    if(contents[c][r]!.Equals(highlightedContent))
    {
        highLightStart = (r, contentLine.Length); 
        contentLine += $" {contents[c][r]!.ToString()!.PadRight(padding)} ║";
        highLightEnd = (r, contentLine.Length-1);
    }
    */

    public void AddContentRow<T>(List<T> contents, bool dynamicSpacing = false) where T : class
    {
        AddContentRow([contents], dynamicSpacing);
    }


    public void AddHeader(List<string> headerContents)
    {
        header.Clear();
        var nColumns = headerContents.Count;
        var columnWidth = tableWidth/nColumns;
        var headerText = "║";

        foreach (var header in headerContents)
        {
            headerText += $" {header.PadRight(columnWidth - 2)} ║";
        }

        var topBar = DrawTableUtil.CreateCorneredLine(nColumns, columnWidth, Position.Top);
        var botBar = DrawTableUtil.CreateCorneredLine(nColumns, columnWidth, Position.Mid);

        header.Add(topBar);
        header.Add(headerText);
        header.Add(botBar);
    }

    public void AddFooter(List<string> footerContents)
    {
        footer.Clear();
        var nColumns = footerContents.Count;
        var columnWidth = tableWidth/nColumns;
        var footerText = "║";

        foreach (var footer in footerContents)
        {
            footerText += $" {footer.PadRight(columnWidth - 1)} ║";
        }

        var botBar = DrawTableUtil.CreateCorneredLine(tableWidth, Position.Bot);

        footer.Add(footerText);
        footer.Add(botBar);
    }

    public void PrintTable()
    {
        FillCorners();

        header.ForEach(Console.WriteLine);

        for (int r = 0; r < contentRows.Count; r++)
        {
            if(r == highLightStart.Item1)
            {
                Console.WriteLine(contentRows[r][..highLightStart.Item2]);
                
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(contentRows[r].Substring(highLightStart.Item2, highLightEnd.Item2));

                Console.ResetColor();
                Console.WriteLine(contentRows[r][highLightEnd.Item2..]);
            }
            else
            {
                Console.WriteLine(contentRows[r]);
            }
        }
        footer.ForEach(Console.WriteLine);
    }

    public void FillCorners()
    {
        if(header.Count == 0)
        {
            header.Add(DrawTableUtil.CreateCorneredLine(tableWidth, Position.Top));
        }
        
        if(footer.Count == 0)
        {
            contentRows.RemoveAt(contentRows.Count-1);
            footer.Add(DrawTableUtil.CreateCorneredLine(tableWidth, Position.Bot));
        }
    }

    public void PrintNextTo(UITable t2, int offset = 10)
    {
        var tables = new List<string>();
        FillCorners();
        t2.FillCorners();
        
        var allLines1 = header.Concat(contentRows).Concat(footer).ToList();
        var allLines2 = t2.header.Concat(t2.contentRows).Concat(t2.footer).ToList();
        var c1 = allLines1.Count;
        var c2 = allLines2.Count;
        var maxInd = int.Max(c1, c2);

        for (int i = 0; i < maxInd; i++)
        {
            var curLine = "";

            if(i < allLines1.Count)
            {
                curLine += allLines1[i];
                curLine += new string(' ', offset);
            }
            else
            {
                curLine += curLine += new string(' ', tableWidth + offset +3);
            }
            
            if(i < allLines2.Count)
            {
                curLine +=  allLines2[i];
            }

            tables.Add(curLine);
        }

        tables.ForEach(Console.WriteLine);
    }
    
    private int[] GetColumnSpacing<T>(List<List<T>> contents, int columnWidth, bool dynamicSpacing) where T : class
    {
        var minimumSpacing = new int[contents.Count];
        Debug.WriteLine($"Colwidth: {columnWidth}");

        if(!dynamicSpacing)
        {
            Array.Fill(minimumSpacing, columnWidth);
            
            foreach (var item in minimumSpacing)
            {
                Debug.WriteLine($"Item: {item}");
            } 
        }
        else
        {
            for (int i = 0; i < contents[0].Count; i++)
            {
                for (int j = 0; j < contents.Count; j++)
                {
                    var lineLen = contents[j][i]!.ToString()!.Length;

                    if(minimumSpacing[i] < lineLen) 
                    {
                        minimumSpacing[i] = lineLen;
                    }
                }               
            }
        }

        return minimumSpacing;
    }
}

