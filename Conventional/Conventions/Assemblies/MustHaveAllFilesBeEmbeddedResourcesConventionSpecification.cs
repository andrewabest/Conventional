using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Conventional.Conventions.Assemblies
{
    public class MustHaveAllFilesBeEmbeddedResourcesConventionSpecification : AssemblyConventionSpecification
    {
        private readonly Regex _fileMatchRegex;
        private readonly string _friendlyDescription;

        private const string EndsWithExtensionPattern = @"\.{0}$";

        public MustHaveAllFilesBeEmbeddedResourcesConventionSpecification(string fileExtension)
        {
            _fileMatchRegex = BuildRegExFromFileExtensions(fileExtension);
            _friendlyDescription = fileExtension;
        }

        public MustHaveAllFilesBeEmbeddedResourcesConventionSpecification(Regex fileMatchRegex)
        {
            _fileMatchRegex = fileMatchRegex;
            _friendlyDescription = fileMatchRegex.ToString();
        }

        protected override string FailureMessage
        {
            get
            {
                return "All files matching '{0}' within assembly '{1}' must have their build action set to 'Embedded Resource'";
            }
        }

        protected override ConventionResult IsSatisfiedByInternal(string assemblyName, XDocument projectDocument)
        {
            var failures = projectDocument
                    .Elements().Single(x => x.Name.LocalName == "Project")
                    .Elements().Where(x => x.Name.LocalName == "ItemGroup")
                    .SelectMany(x => x.Elements().Select(element => new ItemGroupItem(element)))
                    .Where(itemGroupItem => itemGroupItem.MatchesPatternAndIsNotAnEmbeddedResourceOrReference(_fileMatchRegex))
                    .ToArray();

            if (failures.Any())
            {
                var failureText = FailureMessage.FormatWith(_friendlyDescription, assemblyName) +
                                  Environment.NewLine +
                                  string.Join(Environment.NewLine, failures.Select(itemGroupItem => "- " + itemGroupItem.ToString()));

                return ConventionResult.NotSatisfied(assemblyName, failureText);
            }

            return ConventionResult.Satisfied(assemblyName);
        }

        private Regex BuildRegExFromFileExtensions(string fileExtension)
        {
            // Note: fileExtension may be *.sql or sql so handle both cases
            var fileExtensionWithoutLeadingPeriodOrWildcard = fileExtension
                .TrimStart('*')
                .TrimStart('.');

            var regExPattern = EndsWithExtensionPattern.FormatWith(fileExtensionWithoutLeadingPeriodOrWildcard);

            return new Regex(regExPattern, RegexOptions.IgnoreCase);
        }

        private class ItemGroupItem
        {
            private readonly string _type;
            private readonly string _include;

            public ItemGroupItem(XElement itemGroupItemAsXml)
            {
                _type = itemGroupItemAsXml.Name.LocalName;

                var includeAttribute = itemGroupItemAsXml.Attributes().SingleOrDefault(a => a.Name.LocalName == "Include");
                _include = includeAttribute == null ? "" : includeAttribute.Value;
            }

            public bool MatchesPatternAndIsNotAnEmbeddedResourceOrReference(Regex fileMatchRegex)
            {
                if (_type == "EmbeddedResource") return false;
                if (_type == "Reference") return false;

                return fileMatchRegex.IsMatch(_include);
            }

            public override string ToString()
            {
                return "{0} [type={1}]".FormatWith(_include, _type);
            }
        }
    }
}