using System;

namespace Conventional.Conventions
{
    public class MustHaveMatchingEmbeddedResourcesConventionSpecification : ConventionSpecification
    {
        private readonly Func<Type, string> _resourceNameMatcher;

        protected override string FailureMessage
        {
            get
            {
                return "Type {0} must have embedded resource {1}";
            }
        }

        public MustHaveMatchingEmbeddedResourcesConventionSpecification(string extension)
        {
            _resourceNameMatcher = t =>
            {
                // Note: Support both wildcard and non-wildcard extensions
                var fileExtensionWithoutLeadingPeriodOrWildcard = extension
                    .TrimStart('*')
                    .TrimStart('.');
                return string.Join(".", t.FullName, fileExtensionWithoutLeadingPeriodOrWildcard);
            };
        }

        public MustHaveMatchingEmbeddedResourcesConventionSpecification(Func<Type, string> resourceNameMatcher)
        {
            _resourceNameMatcher = resourceNameMatcher;
        }

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var name = _resourceNameMatcher(type);
            using (var manifestResourceStream = type.Assembly.GetManifestResourceStream(name))
            {
                if (manifestResourceStream == null)
                {
                    return ConventionResult.NotSatisfied(type.FullName, FailureMessage.FormatWith(type.FullName, name));
                }
            }
            return ConventionResult.Satisfied(type.FullName);
        }
    }
}