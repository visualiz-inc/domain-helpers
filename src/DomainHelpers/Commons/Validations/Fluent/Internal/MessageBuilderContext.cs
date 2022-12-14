using DomainHelpers.Core.Validations.Validators;

namespace DomainHelpers.Core.Validations.Internal;

public interface IMessageBuilderContext<T, out TProperty> {
    IRuleComponent<T, TProperty> Component { get; }
    IPropertyValidator PropertyValidator { get; }
    ValidationContext<T> ParentContext { get; }
    string PropertyName { get; }
    string DisplayName { get; }
    MessageFormatter MessageFormatter { get; }
    T InstanceToValidate { get; }
    TProperty PropertyValue { get; }
    string GetDefaultMessage();
}

public class MessageBuilderContext<T, TProperty> : IMessageBuilderContext<T, TProperty> {
    public MessageBuilderContext(ValidationContext<T> innerContext, TProperty value,
        RuleComponent<T, TProperty> component) {
        ParentContext = innerContext;
        PropertyValue = value;
        Component = component;
    }

    public RuleComponent<T, TProperty> Component { get; }

    IRuleComponent<T, TProperty> IMessageBuilderContext<T, TProperty>.Component => Component;

    public IPropertyValidator PropertyValidator
        => Component.Validator;

    public ValidationContext<T> ParentContext { get; }

    // public IValidationRule<T> Rule => _innerContext.Rule;

    public string PropertyName => ParentContext.PropertyName;

    public string DisplayName => ParentContext.DisplayName;

    public MessageFormatter MessageFormatter => ParentContext.MessageFormatter;

    public T InstanceToValidate => ParentContext.InstanceToValidate;
    public TProperty PropertyValue { get; }

    public string GetDefaultMessage() {
        return Component.GetErrorMessage(ParentContext, PropertyValue);
    }
}