public interface IDateTimeProvider {
  public DateTime Now {get;}
  public string IsoWeek {get;}
  public string Year_IsoWeek {get;}
}

public class DateTimeProvider : IDateTimeProvider {
  private DateTime _now;
  public DateTimeProvider()
  {
    _now = DateTime.Parse("2023-12-01");
  }
  public DateTime Now 
  {
    get
    {
      _now = _now.AddMilliseconds(157);
      return _now;
    }
  }
  public string IsoWeek => System.Globalization.ISOWeek.GetWeekOfYear(Now).ToString("D2");
  public string Year_IsoWeek => $"{Now:yyyy}_{IsoWeek}";
}