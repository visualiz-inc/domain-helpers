using System.Runtime.CompilerServices;

namespace DomainHelpers.Commons.Collections.Extensions;
internal class ImmutableDictionaryExtensions {
    /// <summary>
    /// Initialize immutable map with key and value pairs.
    /// </summary>
    /// <typeparam name="TKey">Key type.</typeparam>
    /// <typeparam name="TValue">Item type.</typeparam>
    /// <param name="pairs">Initail values.</param>
    /// <returns>The cretead immutable map instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ImmutableDictionary<TKey, TValue> ImmutabbleDictionaryOf<TKey, TValue>(
        params (TKey, TValue)[] pairs)
        where TKey : notnull {
        var map = ImmutableDictionary.CreateBuilder<TKey, TValue>();
        foreach ((var k, var v) in pairs) {
            map.Add(k, v!);
        }

        return map.ToImmutable();
    }
}