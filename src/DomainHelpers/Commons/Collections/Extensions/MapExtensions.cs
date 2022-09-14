using System.Runtime.CompilerServices;

namespace System.Collections.Generic; 

internal class MapExtensions {
    /// <summary>
    ///     Initialize map with key and value pairs.
    /// </summary>
    /// <typeparam name="TKey">Key type.</typeparam>
    /// <typeparam name="TValue">Item type.</typeparam>
    /// <param name="pairs">Initail values.</param>
    /// <returns>The cretead map instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Map<TKey, TValue> MapOf<TKey, TValue>(params (TKey, TValue)[] pairs)
        where TKey : notnull {
        Map<TKey, TValue> map = new Map<TKey, TValue>();
        foreach ((TKey k, TValue v) in pairs) {
            map.Add(k, v!);
        }

        return map;
    }
}