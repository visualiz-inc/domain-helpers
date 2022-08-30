using System.Reflection;

namespace DomainHelpers.Core.Validations.Validators {
    public class EnumValidator<T, TProperty> : PropertyValidator<T, TProperty> {
        private readonly Type _enumType = typeof(TProperty);

        public override string Name => "EnumValidator";

        public override bool IsValid(ValidationContext<T> context, TProperty value) {
            if (value == null) {
                return true;
            }

            Type underlyingEnumType = Nullable.GetUnderlyingType(_enumType) ?? _enumType;

            if (!underlyingEnumType.IsEnum) {
                return false;
            }

            if (underlyingEnumType.GetCustomAttribute<FlagsAttribute>() != null) {
                return IsFlagsEnumDefined(underlyingEnumType, value);
            }

            return Enum.IsDefined(underlyingEnumType, value);
        }

        private static bool IsFlagsEnumDefined(Type enumType, object value) {
            string typeName = Enum.GetUnderlyingType(enumType).Name;

            switch (typeName) {
                case "Byte": {
                    byte typedValue = (byte)value;
                    return EvaluateFlagEnumValues(typedValue, enumType);
                }

                case "Int16": {
                    short typedValue = (short)value;

                    return EvaluateFlagEnumValues(typedValue, enumType);
                }

                case "Int32": {
                    int typedValue = (int)value;

                    return EvaluateFlagEnumValues(typedValue, enumType);
                }

                case "Int64": {
                    long typedValue = (long)value;

                    return EvaluateFlagEnumValues(typedValue, enumType);
                }

                case "SByte": {
                    sbyte typedValue = (sbyte)value;

                    return EvaluateFlagEnumValues(Convert.ToInt64(typedValue), enumType);
                }

                case "UInt16": {
                    ushort typedValue = (ushort)value;
                    return EvaluateFlagEnumValues(typedValue, enumType);
                }

                case "UInt32": {
                    uint typedValue = (uint)value;
                    return EvaluateFlagEnumValues(typedValue, enumType);
                }

                case "UInt64": {
                    ulong typedValue = (ulong)value;
                    return EvaluateFlagEnumValues((long)typedValue, enumType);
                }

                default:
                    string message = $"Unexpected typeName of '{typeName}' during flags enum evaluation.";
                    throw new ArgumentOutOfRangeException(nameof(enumType), message);
            }
        }

        private static bool EvaluateFlagEnumValues(long value, Type enumType) {
            long mask = 0;
            foreach (object? enumValue in Enum.GetValues(enumType)) {
                long enumValueAsInt64 = Convert.ToInt64(enumValue);
                if ((enumValueAsInt64 & value) == enumValueAsInt64) {
                    mask |= enumValueAsInt64;
                    if (mask == value) {
                        return true;
                    }
                }
            }

            return false;
        }

        protected override string GetDefaultMessageTemplate(string errorCode) {
            return Localized(errorCode, Name);
        }
    }
}
