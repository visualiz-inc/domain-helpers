namespace DomainHelpers.Core.Validations; 

/// <summary>
///     Specifies how rules should cascade when one fails.
/// </summary>
public enum CascadeMode {
    /// <summary>
    ///     When a rule/validator fails, execution continues to the next rule/validator.
    ///     For more information, see the methods/properties that accept this enum as a parameter.
    /// </summary>
    Continue,

    /// <summary>
    ///     When a rule/validator fails, validation is stopped for the current rule/validator.
    ///     For more information, see the methods/properties that accept this enum as a parameter.
    /// </summary>
    Stop
}

/// <summary>
///     Specifies where a When/Unless condition should be applied
/// </summary>
public enum ApplyConditionTo {
    /// <summary>
    ///     Applies the condition to all validators declared so far in the chain.
    /// </summary>
    AllValidators,

    /// <summary>
    ///     Applies the condition to the current validator only.
    /// </summary>
    CurrentValidator
}

/// <summary>
///     Specifies the severity of a rule.
/// </summary>
public enum Severity {
    /// <summary>
    ///     Error
    /// </summary>
    Error,

    /// <summary>
    ///     Warning
    /// </summary>
    Warning,

    /// <summary>
    ///     Info
    /// </summary>
    Info
}