using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainHelpers.Commons.DateTimeUtils;

public static partial class DateTimeUtils {
    public static DateOnly GetLastDate(this DateTime date) {
        return DateOnly.FromDateTime(date.AddMonths(1).AddDays(-1));
    }
}
