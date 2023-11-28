using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace DomainHelpers.Commons.Primitives;
internal static class RandomProvider {
    [ThreadStatic] private static Random? random;

    [ThreadStatic] private static XorShift64? xorShift;

    // this random is async-unsafe, be careful to use.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Random GetRandom() {
        random ??= CreateRandom();

        return random;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static Random CreateRandom() {
        using var rng = RandomNumberGenerator.Create();
        // Span<byte> buffer = stackalloc byte[sizeof(int)];
        var buffer = new byte[sizeof(int)];
        rng.GetBytes(buffer);
        var seed = BitConverter.ToInt32(buffer, 0);
        return new Random(seed);
    }

    // this random is async-unsafe, be careful to use.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XorShift64 GetXorShift64() {
        xorShift ??= CreateXorShift64();

        return xorShift;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static XorShift64 CreateXorShift64() {
        using var rng = RandomNumberGenerator.Create();
        // Span<byte> buffer = stackalloc byte[sizeof(UInt64)];
        var buffer = new byte[sizeof(ulong)];
        rng.GetBytes(buffer);
        var seed = BitConverter.ToUInt64(buffer, 0);
        return new XorShift64(seed);
    }
}

internal class XorShift64 {
    private ulong x = 88172645463325252UL;

    public XorShift64(ulong seed) {
        if (seed != 0) {
            x = seed;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong Next() {
        x ^= x << 7;
        return x ^= x >> 9;
    }
}