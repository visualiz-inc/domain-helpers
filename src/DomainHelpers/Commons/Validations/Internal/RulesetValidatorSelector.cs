namespace DomainHelpers.Core.Validations.Internal {
    /// <summary>
    ///     Selects validators that belong to the specified rulesets.
    /// </summary>
    public class RulesetValidatorSelector : IValidatorSelector {
        public const string DefaultRuleSetName = "default";
        public const string WildcardRuleSetName = "*";

        /// <summary>
        ///     Creates a new instance of the RulesetValidatorSelector.
        /// </summary>
        public RulesetValidatorSelector(IEnumerable<string> rulesetsToExecute) {
            RuleSets = rulesetsToExecute;
        }

        /// <summary>
        ///     Rule sets
        /// </summary>
        public IEnumerable<string> RuleSets { get; }

        /// <summary>
        ///     Determines whether or not a rule should execute.
        /// </summary>
        /// <param name="rule">The rule</param>
        /// <param name="propertyPath">Property path (eg Customer.Address.Line1)</param>
        /// <param name="context">Contextual information</param>
        /// <returns>Whether or not the validator can execute.</returns>
        public virtual bool CanExecute(IValidationRule rule, string propertyPath, IValidationContext context) {
            HashSet<string> executed =
                context.RootContextData.GetOrAdd("_FV_RuleSetsExecuted", () => new HashSet<string>());

            if ((rule.RuleSets == null || rule.RuleSets.Length == 0) && RuleSets.Any()) {
                if (IsIncludeRule(rule)) {
                    return true;
                }
            }

            if ((rule.RuleSets == null || rule.RuleSets.Length == 0) && !RuleSets.Any()) {
                executed.Add(DefaultRuleSetName);
                return true;
            }

            if (RuleSets.Contains(DefaultRuleSetName, StringComparer.OrdinalIgnoreCase)) {
                if (rule.RuleSets == null || rule.RuleSets.Length == 0 ||
                    rule.RuleSets.Contains(DefaultRuleSetName, StringComparer.OrdinalIgnoreCase)) {
                    executed.Add(DefaultRuleSetName);
                    return true;
                }
            }

            if (rule.RuleSets != null && rule.RuleSets.Length > 0 && RuleSets.Any()) {
                List<string> intersection =
                    rule.RuleSets.Intersect(RuleSets, StringComparer.OrdinalIgnoreCase).ToList();
                if (intersection.Any()) {
                    intersection.ForEach(r => executed.Add(r));
                    return true;
                }
            }

            if (RuleSets.Contains(WildcardRuleSetName)) {
                if (rule.RuleSets == null || rule.RuleSets.Length == 0) {
                    executed.Add(DefaultRuleSetName);
                }
                else {
                    foreach (string r in rule.RuleSets) {
                        executed.Add(r);
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        ///     Checks if the rule is an IncludeRule
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        protected bool IsIncludeRule(IValidationRule rule) {
            return rule is IIncludeRule;
        }

        // Prior to 9.1, FV's primary ruleset syntax allowed specifying multiple rulesets
        // in a single comma-separated string. This approach was deprecated in 9.1 in favour
        // of the options syntax, but we still need this logic in a couple of places for backwards compat.
        internal static string[] LegacyRulesetSplit(string ruleSet) {
            string[] ruleSetNames = ruleSet.Split(',', ';')
                .Select(x => x.Trim())
                .ToArray();

            return ruleSetNames;
        }
    }
}
