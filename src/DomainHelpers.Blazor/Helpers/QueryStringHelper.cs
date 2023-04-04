using System.Collections;
using System.Reflection;

public static class QueryStringHelper {
    public static string Serialize<T>(T obj) where T : class {
        var query = new StringBuilder();
        BuildQueryString(obj, query, "");

        return query.ToString();
    }

    private static void BuildQueryString<T>(T? obj, StringBuilder query, string prefix = "") where T : class {
        if (obj is null) {
            return;
        }

        foreach (var p in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
            var value = p.GetValue(obj);

            if (p.PropertyType == typeof(string))
                query.Append($"&{prefix}{p.Name}={value}");

            else if (p.PropertyType.IsAssignableTo(typeof(IEnumerable)) && value is not null)
                foreach (var item in (IEnumerable)value!)
                    query.Append($"&{prefix}{p.Name}={item.ToString()}");

            else if (p.PropertyType.IsValueType)
                query.Append($"&{prefix}{p.Name}={value}");

            else if (p.PropertyType.IsEnum)
                query.Append($"&{prefix}{p.Name}={value}");

            else if (p.PropertyType.IsClass)
                BuildQueryString(value, query, $"{prefix}{p.Name}.");
        }
    }
}