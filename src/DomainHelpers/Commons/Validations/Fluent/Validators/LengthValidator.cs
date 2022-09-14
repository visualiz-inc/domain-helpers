namespace DomainHelpers.Core.Validations.Validators; 

public class LengthValidator<T> : PropertyValidator<T, string>, ILengthValidator {
    public LengthValidator(int min, int max) {
        Max = max;
        Min = min;

        if (max != -1 && max < min) {
            throw new ArgumentOutOfRangeException(nameof(max), "Max should be larger than min.");
        }
    }

    public LengthValidator(Func<T, int> min, Func<T, int> max) {
        MaxFunc = max;
        MinFunc = min;
    }

    public Func<T, int> MinFunc { get; set; }
    public Func<T, int> MaxFunc { get; set; }
    public override string Name => "LengthValidator";

    public int Min { get; }
    public int Max { get; }

    public override bool IsValid(ValidationContext<T> context, string value) {
        if (value == null) {
            return true;
        }

        int min = Min;
        int max = Max;

        if (MaxFunc != null && MinFunc != null) {
            max = MaxFunc(context.InstanceToValidate);
            min = MinFunc(context.InstanceToValidate);
        }

        int length = value.Length;

        if (length < min || (length > max && max != -1)) {
            context.MessageFormatter
                .AppendArgument("MinLength", min)
                .AppendArgument("MaxLength", max)
                .AppendArgument("TotalLength", length);

            return false;
        }

        return true;
    }

    protected override string GetDefaultMessageTemplate(string errorCode) {
        return Localized(errorCode, Name);
    }
}

public class ExactLengthValidator<T> : LengthValidator<T>, IExactLengthValidator {
    public ExactLengthValidator(int length) : base(length, length) { }

    public ExactLengthValidator(Func<T, int> length)
        : base(length, length) { }

    public override string Name => "ExactLengthValidator";

    protected override string GetDefaultMessageTemplate(string errorCode) {
        return Localized(errorCode, Name);
    }
}

public class MaximumLengthValidator<T> : LengthValidator<T>, IMaximumLengthValidator {
    public MaximumLengthValidator(int max)
        : base(0, max) { }

    public MaximumLengthValidator(Func<T, int> max)
        : base(obj => 0, max) { }

    public override string Name => "MaximumLengthValidator";

    protected override string GetDefaultMessageTemplate(string errorCode) {
        return Localized(errorCode, Name);
    }
}

public class MinimumLengthValidator<T> : LengthValidator<T>, IMinimumLengthValidator {
    public MinimumLengthValidator(int min)
        : base(min, -1) { }

    public MinimumLengthValidator(Func<T, int> min)
        : base(min, obj => -1) { }

    public override string Name => "MinimumLengthValidator";

    protected override string GetDefaultMessageTemplate(string errorCode) {
        return Localized(errorCode, Name);
    }
}

public interface ILengthValidator : IPropertyValidator {
    int Min { get; }
    int Max { get; }
}

public interface IMaximumLengthValidator : ILengthValidator { }

public interface IMinimumLengthValidator : ILengthValidator { }

public interface IExactLengthValidator : ILengthValidator { }