namespace DomainHelpers.Commons.Enums;

public static class EnumUtils {
    public static IEnumerable<(string Name, TEnum Value)> GetNameAndValues<TEnum>()
        where TEnum : struct, Enum {
        return Enum.GetNames<TEnum>().Zip(Enum.GetValues<TEnum>());
    }

    public static IEnumerable<(string? Name, TEnum Value)> GetDisplayNameAndValues<TEnum>()
    where TEnum : struct, Enum {
        return Enum.GetValues<TEnum>().Select(x => x.ToDisplayName()).Zip(Enum.GetValues<TEnum>());
    }
}