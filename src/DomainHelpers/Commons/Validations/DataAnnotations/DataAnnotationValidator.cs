using DomainHelpers.Commons.DataAnnotations;
using DomainHelpers.Core.Commons;
using DomainHelpers.Core.Commons.Validations;
using System.ComponentModel.DataAnnotations;

namespace DomainHelpers.DataAnnotations {
    public static class DataAnnotationValidator {
        /// <summary>
        ///     Validate the object with data annotaion.
        /// </summary>
        /// <typeparam name="T">The target object type.</typeparam>
        /// <param name="obj">The target object.</param>
        /// <returns>The validation result.</returns>
        public static (bool, ImmutableArray<ValidationResult>) ValidateWithAnnotation(this object? obj) {
            List<ValidationResult> results = new();
            ValidationContext context = new(obj, null, null);
            bool isSuccess = Validator.TryValidateObject(obj, context, results, true);
            return (isSuccess, results.ToImmutableArray());
        }

        /// <summary>
        ///     Validate the object with data annotaion.
        /// </summary>
        /// <typeparam name="T">The target object type.</typeparam>
        /// <param name="obj">The target object.</param>
        /// <param name="displayMessage">The display message to create <see cref="GeneralException" /> </param>
        /// <returns>The target object.</returns>
        /// <exception cref="DataValidationException"> The exception throws validation error.</exception>
        public static T ValidateAndThrowIfInvalid<T>(this T obj, string? displayMessage = null)
            where T : class {
            if (obj?.ValidateWithAnnotation() is (false, var messages)) {
                string message = messages.GetErrorMesssages().JoinStrings("\r\n");
                throw new DataValidationException(
                    new DataValidationExceptionType(messages),
                    message,
                    displayMessage ?? message,
                    Ulid.NewUlid()
                );
            }

            return obj!;
        }

        public static ImmutableArray<ValidationResult> FluttenNesteds(this IEnumerable<ValidationResult> messages) {
            return messages
                .SelectMany(
                    x => x is CompositeValidationResult c
                        ? ArrayOfRange(c.Results.FluttenNesteds())
                        : ArrayOf(x)
                )
                .ToImmutableArray();
        }

        public static IEnumerable<string> GetErrorMesssages(this IEnumerable<ValidationResult> messages) {
            return messages
                .SelectMany(
                    x => x is CompositeValidationResult c
                        ? c.Results.Select(x => x.ErrorMessage)
                        : ArrayOf(x.ErrorMessage)
                );
        }

        public static ImmutableDictionary<string, string?[]> BuildErrorMessageTree(
            this IEnumerable<ValidationResult> messages) {
            ImmutableDictionary<string, List<string?>>.Builder result =
                ImmutableDictionary.CreateBuilder<string, List<string?>>();

            ImmutableArray<ValidationResult> s = messages.FluttenNesteds();
            foreach (ValidationResult message in messages.FluttenNesteds()) {
                foreach (string? member in message.MemberNames) {
                    string memberName = member ?? "_";
                    if (result.ContainsKey(memberName)) {
                        result[memberName].Add(message.ErrorMessage);
                    }
                    else {
                        result.Add(
                            memberName,
                            new List<string?> { message.ErrorMessage }
                        );
                    }
                }
            }

            return result.Select(x => new KeyValuePair<string, string?[]>(x.Key, x.Value.ToArray()))
                .ToImmutableDictionary();
        }
    }
}
