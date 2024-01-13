namespace DomainHelpers.Commons.DateTimeUtils;

public static partial class DateTimeUtils {
    public static DateOnly GetLastDateOfMonth(this DateTime date) {
        return new DateOnly(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
    }
}