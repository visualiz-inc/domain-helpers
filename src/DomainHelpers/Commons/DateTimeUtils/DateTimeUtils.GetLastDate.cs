namespace DomainHelpers.Commons.DateTimeUtils;

public static partial class DateTimeUtils {
    public static DateOnly GetLastDate(this DateTime date) {
        return DateOnly.FromDateTime(date.AddMonths(1).AddDays(-1));
    }
}