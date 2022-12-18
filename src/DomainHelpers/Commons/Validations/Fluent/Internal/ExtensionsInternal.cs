using DomainHelpers.Core.Validations.Resources;
using System.Linq.Expressions;

namespace DomainHelpers.Core.Validations.Internal;

internal static class ExtensionsInternal {
    internal static void Guard(this object obj, string message, string paramName) {
        if (obj == null) {
            throw new ArgumentNullException(paramName, message);
        }
    }

    internal static void Guard(this string str, string message, string paramName) {
        if (str == null) {
            throw new ArgumentNullException(paramName, message);
        }

        if (string.IsNullOrEmpty(str)) {
            throw new ArgumentException(message, paramName);
        }
    }

    /// <summary>
    /// Checks if the expression is a parameter expression
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    internal static bool IsParameterExpression(this LambdaExpression expression) {
        return expression.Body.NodeType == ExpressionType.Parameter;
    }

    /// <summary>
    /// Splits pascal case, so "FooBar" would become "Foo Bar".
    /// </summary>
    /// <remarks>
    /// Pascal case strings with periods delimiting the upper case letters,
    /// such as "Address.Line1", will have the periods removed.
    /// </remarks>
    internal static string SplitPascalCase(this string input) {
        if (string.IsNullOrEmpty(input)) {
            return input;
        }

        StringBuilder retVal = new StringBuilder(input.Length + 5);

        for (int i = 0; i < input.Length; ++i) {
            char currentChar = input[i];
            if (char.IsUpper(currentChar)) {
                if ((i > 1 && !char.IsUpper(input[i - 1]))
                    || (i + 1 < input.Length && !char.IsUpper(input[i + 1]))) {
                    retVal.Append(' ');
                }
            }

            if (!Equals('.', currentChar)
                || i + 1 == input.Length
                || !char.IsUpper(input[i + 1])) {
                retVal.Append(currentChar);
            }
        }

        return retVal.ToString().Trim();
    }

    internal static T GetOrAdd<T>(this IDictionary<string, object> dict, string key, Func<T> value) {
        if (dict.TryGetValue(key, out object? tmp)) {
            if (tmp is T result) {
                return result;
            }
        }

        T val = value();
        dict[key] = val;
        return val;
    }

    internal static string ResolveErrorMessageUsingErrorCode(this ILanguageManager languageManager,
        string errorCode, string fallbackKey) {
        if (errorCode != null) {
            string result = languageManager.GetString(errorCode);

            if (!string.IsNullOrEmpty(result)) {
                return result;
            }
        }

        return languageManager.GetString(fallbackKey);
    }
}