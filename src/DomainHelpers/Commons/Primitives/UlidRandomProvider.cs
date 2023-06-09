﻿using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace System;

internal static class RandomProvider {
    [ThreadStatic] private static Random? random;

    [ThreadStatic] private static XorShift64? xorShift;

    // this random is async-unsafe, be careful to use.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Random GetRandom() {
        if (random is null) {
            random = CreateRandom();
        }

        return random;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static Random CreateRandom() {
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create()) {
            // Span<byte> buffer = stackalloc byte[sizeof(int)];
            byte[] buffer = new byte[sizeof(int)];
            rng.GetBytes(buffer);
            int seed = BitConverter.ToInt32(buffer, 0);
            return new Random(seed);
        }
    }

    // this random is async-unsafe, be careful to use.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XorShift64 GetXorShift64() {
        if (xorShift == null) {
            xorShift = CreateXorShift64();
        }

        return xorShift;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static XorShift64 CreateXorShift64() {
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create()) {
            // Span<byte> buffer = stackalloc byte[sizeof(UInt64)];
            byte[] buffer = new byte[sizeof(ulong)];
            rng.GetBytes(buffer);
            ulong seed = BitConverter.ToUInt64(buffer, 0);
            return new XorShift64(seed);
        }
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
        x = x ^ (x << 7);
        return x = x ^ (x >> 9);
    }
}