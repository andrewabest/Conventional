using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Conventional.Extensions;

namespace Conventional.Conventions.Assemblies
{
    public class MustHaveAllFilesBeResourcesConventionSpecification : AssemblyConventionSpecification
    {
        private readonly Regex _fileMatchRegex;
        private readonly string _friendlyDescription;

        private const string EndsWithExtensionPattern = @"\.{0}$";

        public MustHaveAllFilesBeResourcesConventionSpecification(string fileExtension)
        {
            _fileMatchRegex = BuildRegExFromFileExtensions(fileExtension);
            _friendlyDescription = fileExtension;
        }

        public MustHaveAllFilesBeResourcesConventionSpecification(Regex fileMatchRegex)
        {
            _fileMatchRegex = fileMatchRegex;
            _friendlyDescription = fileMatchRegex.ToString();
        }

        protected override string FailureMessage => "All files matching '{0}' within assembly '{1}' must have their build action set to '{2}'";

        protected override ConventionResult IsSatisfiedByLegacyCsprojFormat(string assemblyName, XDocument projectDocument)
        {
            var failures = ItemGroupItem.FromProjectDocument(projectDocument)
                .Where(itemGroupItem => itemGroupItem.MatchesPatternAndIsNotAnResourceOrReference(_fileMatchRegex))
                .Select(itemGroupItem => itemGroupItem.ToString())
                .ToArray();

            return BuildResult(assemblyName, failures, "Resource");
        }

        protected override ConventionResult IsSatisfiedBy(string assemblyName, XDocument projectDocument)
        {
            var children = ItemGroupItem.FromProjectDocument(projectDocument).ToArray();

            var projectFiles =
                DirectoryEx
                    .GetFilesExceptOutput(ProjectFolder, "*")
                    .Where(x => _fileMatchRegex.IsMatch(x))
                    .Select(x => x.Replace($"{ProjectFolder}\\", ""))
                    .ToArray();

            var failures =
                children
                    .Where(itemGroupItem => itemGroupItem.MatchesPatternAndIsNotAnEmbeddedResource(_fileMatchRegex))
                    .Select(itemGroupItem => itemGroupItem.ToString())
                    .Union(projectFiles.Where(x => children.None(child => child.Update.Equals(x))))
                    .ToArray();

            return BuildResult(assemblyName, failures, "Embedded Resource");
        }

        private ConventionResult BuildResult(string assemblyName, string[] failures, string buildAction)
        {
            if (failures.Any())
            {
                var failureText = FailureMessage.FormatWith(_friendlyDescription, assemblyName, buildAction) +
                                  Environment.NewLine +
                                  string.Join(Environment.NewLine, failures.Select(itemGroupItem => "- " + itemGroupItem.ToString()));

                return ConventionResult.NotSatisfied(assemblyName, failureText);
            }

            return ConventionResult.Satisfied(assemblyName);
        }

        private static Regex BuildRegExFromFileExtensions(string fileExtension)
        {
            // Note: fileExtension may be *.sql or sql so handle both cases
            var fileExtensionWithoutLeadingPeriodOrWildcard = fileExtension
                .TrimStart('*')
                .TrimStart('.');

            var regExPattern = EndsWithExtensionPattern.FormatWith(fileExtensionWithoutLeadingPeriodOrWildcard);

            return new Regex(regExPattern, RegexOptions.IgnoreCase);
        }
    }
}