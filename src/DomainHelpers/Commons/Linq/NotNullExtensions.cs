using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainHelpers.Commons.Linq;

public static class NotNullExtensions {
    public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> seq) => seq.Where(x => x != null)!;
}
