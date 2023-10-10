using FluentValidation;

namespace SimpleWebApp.Validators
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilder<T, int> InvalidAge<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            var options = ruleBuilder
                .NotNull()
                .Must(age => age > 0);

            return options;
        }
    }
}
