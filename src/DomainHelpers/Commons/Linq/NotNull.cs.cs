using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainHelpers.Commons.Linq;

public static class NotNullExtension {
    public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> sequence) where T : class? {
        return sequence.Where(x => x is not null)!;
    }

    public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> sequence) where T : struct {
        return sequence.Where(x => x is not null).Select(x => x ?? default);
    }
}
