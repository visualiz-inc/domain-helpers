using System.Text.RegularExpressions;

namespace DomainHelpers.Core.Validations.Validators;

public class RegularExpressionValidator<T> : PropertyValidator<T, string>, IRegularExpressionValidator {
    private readonly Func<T, Regex> _regexFunc;

    public RegularExpressionValidator(string expression) {
        Expression = expression;

        Regex regex = CreateRegex(expression);
        _regexFunc = x => regex;
    }

    public RegularExpressionValidator(Regex regex) {
        Expression = regex.ToString();
        _regexFunc = x => regex;
    }

    public RegularExpressionValidator(string expression, RegexOptions options) {
        Expression = expression;
        Regex regex = CreateRegex(expression, options);
        _regexFunc = x => regex;
    }

    public RegularExpressionValidator(Func<T, string> expressionFunc) {
        _regexFunc = x => CreateRegex(expressionFunc(x));
    }

    public RegularExpressionValidator(Func<T, Regex> regexFunc) {
        _regexFunc = regexFunc;
    }

    public RegularExpressionValidator(Func<T, string> expression, RegexOptions options) {
        _regexFunc = x => CreateRegex(expression(x), options);
    }

    public override string Name => "RegularExpressionValidator";

    public string Expression { get; }

    public override bool IsValid(ValidationContext<T> context, string value) {
        Regex? regex = _regexFunc(context.InstanceToValidate);

        if (regex != null && value != null && !regex.IsMatch(value)) {
            context.MessageFormatter.AppendArgument("RegularExpression", regex.ToString());
            return false;
        }

        return true;
    }

    private static Regex CreateRegex(string expression, RegexOptions options = RegexOptions.None) {
        return new Regex(expression, options, TimeSpan.FromSeconds(2.0));
    }

    protected override string GetDefaultMessageTemplate(string errorCode) {
        return Localized(errorCode, Name);
    }
}

public interface IRegularExpressionValidator : IPropertyValidator {
    string Expression { get; }
}