using System.ComponentModel.DataAnnotations;
using System.Globalization;

// TODO 
// ToString() dates
public class Transaction
{
    public int TransactionId { get; set; }
    public string Name {get; set;} = "";
    public DateTime Date {get; set;}
    public int Amount {get; set;}
    public int UserId {get; set;}

    //public int CategoryId {get; set;}
    //public Category? Category {get; set;}

    public int GetWeekNumber(){
        CalendarWeekRule weekRule = CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule;
        DayOfWeek firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
        return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(Date, weekRule, firstDayOfWeek);
    }

    public override string ToString()
    {
        return $"{Name} {Amount}kr {DateUtils.ToStringWithoutTime(Date)}";
    }
}
