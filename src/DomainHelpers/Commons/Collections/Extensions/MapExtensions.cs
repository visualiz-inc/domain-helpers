using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic;

internal class MapExtensions {
    /// <summary>
    /// Initialize map with key and value pairs.
    /// </summary>
    /// <typeparam name="TKey">Key type.</typeparam>
    /// <typeparam name="TValue">Item type.</typeparam>
    /// <param name="pairs">Initail values.</param>
    /// <returns>The cretead map instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Map<TKey, TValue> MapOf<TKey, TValue>(params (TKey, TValue)[] pairs)
    where TKey : notnull {
        var map = new Map<TKey, TValue>();
        foreach (var (k, v) in pairs) {
            map.Add(k, v!);
        }

        return map;
    }
}
