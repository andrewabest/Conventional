using System;

namespace Conventional.Conventions
{
    public class MustHaveMatchingEmbeddedResourcesConventionSpecification : ConventionSpecification
    {
        private readonly string _extension;

        public MustHaveMatchingEmbeddedResourcesConventionSpecification(string extension)
        {
            _extension = extension;
        }

        protected override string FailureMessage
        {
            get { return "Type {0} must have embedded resources with matching name (expected {1})"; }
        }

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var resourceName = type.FullName + _extension;
            using (var stream = type.Assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    return ConventionResult.NotSatisfied(type.FullName, FailureMessage.FormatWith(type.FullName, resourceName));
                }
            }

            return ConventionResult.Satisfied(type.FullName);
        }
    }
}