﻿using System.Runtime.CompilerServices;

namespace DomainHelpers.Commons.Text;

public static class StringExtensions {
    /// <summary>
    /// Concatenates the members of a constructed System.Collections.Generic.IEnumerable`1
    /// collection of type System.String, using the specified separator between each
    /// member.
    /// </summary>
    /// <param name="texts"> A collection that contains the strings to concatenate.</param>
    /// <param name="separator">
    /// The string to use as a separator.separator is included in the returned string
    /// only if values has more than one element.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// texts is null.
    /// </exception>
    /// <exception cref="OutOfMemoryException">
    /// The length of the resulting string overflows the maximum allowed length (System.Int32.MaxValue).
    /// </exception>
    /// <returns>values is null.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string JoinStrings(this IEnumerable<string?> texts, string separator) {
        return string.Join(separator, texts);
    }

    /// <summary>
    /// Concatenates the members of a constructed System.Collections.Generic.IEnumerable`1
    /// collection of type System.String, using the specified separator between each
    /// member.
    /// </summary>
    /// <param name="texts"> A collection that contains the strings to concatenate.</param>
    /// <param name="separator">
    /// The string to use as a separator.separator is included in the returned string
    /// only if values has more than one element.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// texts is null.
    /// </exception>
    /// <exception cref="OutOfMemoryException">
    /// The length of the resulting string overflows the maximum allowed length (System.Int32.MaxValue).
    /// </exception>
    /// <returns>values is null.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string JoinStrings(this IEnumerable<string?> texts, char separator) {
        return string.Join(separator, texts);
    }

    /// <summary>
    /// Reteat string.
    /// </summary>
    /// <param name="str">The target string.</param>
    /// <param name="repeat"></param>
    /// <returns>The value.</returns>
    public static string Repeat(this string str, int repeat) {
        var builder = new StringBuilder();

        for (var i = 0; i < repeat; i++) {
            builder.Append(str);
        }

        return builder.ToString();
    }
}