using System.Globalization;

// TODO 
// Add conversions for each date format
// 1. MM/DD/YYYY - Example: 01/17/2025
// 2. DD/MM/YYYY - Example: 17/01/2025
// 3. Month DD, YYYY - Example: January 17, 2025
// 4. YYYY/MM/DD - Example: 2025/01/17
// 5. DD-MMM-YYYY - Example: 17-Jan-2025
// 6. MMM DD, YYYY - Example: Jan 17, 2025
public static class DateUtils
{

    public static DateTime TryConvertDate(string dateString, out bool success)
    {
        success = false;
        return DateTime.UtcNow;
    }

    public static string ToStringWithoutTime(DateTime date)
    {
        return $"{date.Day}/{date.Month}";
    }

    public static int GetCurrentWeek()
    {
        return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }
}