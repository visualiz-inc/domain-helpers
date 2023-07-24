using System.Reflection;

namespace DomainHelpers.Commons.Enums; 
public static class EnumExtensions {
    public static IEnumerable<(string Name, TEnum Value)> GetNameAndValues<TEnum>()
        where TEnum : struct, Enum {
        return Enum.GetNames<TEnum>().Zip(Enum.GetValues<TEnum>());
    }

    public static string? ToDisplayName(this Enum e) {
        return e.GetType().GetField(e.ToString())?.GetCustomAttribute<EnumDisplayNameAttribute>()?.DisplayName ?? "";
    }

    public static TEnum CastTo<TEnum>(this Enum e, bool ignoreCase = true)
        where TEnum : struct, Enum =>
            Enum.TryParse<TEnum>(e.ToString(), ignoreCase, out var result)
                ? result
                : throw new InvalidCastException($"Cannot cast {e.GetType()} to {typeof(TEnum)}");
}