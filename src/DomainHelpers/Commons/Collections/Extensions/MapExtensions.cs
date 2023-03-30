using System.Runtime.CompilerServices;

namespace DomainHelpers.Commons.Collections.Extensions;

public class MapExtensions {
    /// <summary>
    /// Initialize map with key and value pairs.
    /// </summary>
    /// <typeparam name="TKey">Key type.</typeparam>
    /// <typeparam name="TValue">Item type.</typeparam>
    /// <param name="pairs">Initail values.</param>
    /// <returns>The cretead map instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NullableMap<TKey, TValue> NullableMapOf<TKey, TValue>(params (TKey, TValue)[] pairs)
        where TKey : notnull
        where TValue : class {
        var map = new NullableMap<TKey, TValue>();
        foreach ((var k, var v) in pairs) {
            map.Add(k, v!);
        }

        return map;
    }
}