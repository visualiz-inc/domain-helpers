using System.Reflection;

namespace System;

public static class EnumExtenstions {
    public static IEnumerable<(string Name, TEnum Value)> GetNameAndValues<TEnum>()
        where TEnum : struct, Enum {
        return Enum.GetNames<TEnum>().Zip(Enum.GetValues<TEnum>());
    }

    public static string? ToDisplayName(this Enum e) {
        return e.GetType().GetField(e.ToString())?.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? "";
    }
}
