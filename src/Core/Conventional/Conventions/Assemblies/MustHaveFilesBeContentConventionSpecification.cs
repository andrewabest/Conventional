using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Conventional.Extensions;

namespace Conventional.Conventions.Assemblies
{
    public class MustHaveFilesBeContentConventionSpecification : AssemblyConventionSpecification
    {
        private readonly Regex _fileMatchRegex;
        private readonly string _friendlyDescription;
        private const string EndsWithExtensionPattern = @"\.{0}$";

        public MustHaveFilesBeContentConventionSpecification(string fileExtension)
        {
            _fileMatchRegex = BuildRegExFromFileExtensions(fileExtension);
            _friendlyDescription = fileExtension;
        }

        public MustHaveFilesBeContentConventionSpecification(Regex fileMatchRegex)
        {
            _fileMatchRegex = fileMatchRegex;
            _friendlyDescription = fileMatchRegex.ToString();
        }

        protected override string FailureMessage => "All files matching '{0}' within assembly '{1}' must have their build action set to 'Content - Copy if newer'";

        protected override ConventionResult IsSatisfiedByLegacyCsprojFormat(string assemblyName, XDocument projectDocument)
        {
            var failures = ItemGroupItem.FromProjectDocument(projectDocument)
                .Where(itemGroupItem => itemGroupItem.MatchesPatternAndIsNotContentCopyNewest(_fileMatchRegex))
                .Select(itemGroupItem => itemGroupItem.ToString())
                .ToArray();

            return BuildResult(assemblyName, failures);
        }

        protected override ConventionResult IsSatisfiedBy(string assemblyName, XDocument projectDocument)
        {
            var children = ItemGroupItem.FromProjectDocument(projectDocument).ToArray();

            var projectFiles =
                DirectoryEx
                    .GetFilesExceptOutput(ProjectFolder, "*")
                    .Where(x => _fileMatchRegex.IsMatch(x))
                    .Select(x => x.Replace($"{ProjectFolder}{Path.DirectorySeparatorChar}", ""))
                    .ToArray();

            var normalisedIncludes = children.Select(c => c.Include.Replace('\\', Path.DirectorySeparatorChar));

            var failures =
                children
                    .Where(itemGroupItem => itemGroupItem.MatchesPatternAndIsNotContentCopyNewest(_fileMatchRegex))
                    .Select(itemGroupItem => itemGroupItem.ToString())
                    .Union(projectFiles.Where(x => normalisedIncludes.None(include => include.Equals(x))))
                    .ToArray();

            return BuildResult(assemblyName, failures);
        }

        private ConventionResult BuildResult(string assemblyName, string[] failures)
        {
            if (failures.Any())
            {
                var failureText = FailureMessage.FormatWith(_friendlyDescription, assemblyName) +
                                  Environment.NewLine +
                                  string.Join(Environment.NewLine, failures.Select(x => "- " + x));

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