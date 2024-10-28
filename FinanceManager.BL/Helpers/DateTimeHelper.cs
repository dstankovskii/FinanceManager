namespace FinanceManager.BL.Helpers;

public static class DateTimeHelper
{
    public static string ConvertToUtcString(DateTime dateTime)
    {
        var dateTimeOffset = new DateTimeOffset(dateTime, TimeSpan.Zero);

        return dateTimeOffset.ToString("yyyy-MM-ddTHH:mm:sszzz");
    }
}
