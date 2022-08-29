namespace DomainHelpers.Core.Validations.Validators;

using Resources;
using Results;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public abstract class NoopPropertyValidator<T, TProperty> : PropertyValidator<T, TProperty> {
    public override bool IsValid(ValidationContext<T> context, TProperty value) => true;
}
