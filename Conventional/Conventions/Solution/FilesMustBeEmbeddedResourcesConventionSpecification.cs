using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Conventional.Conventions.Solution
{
    public class FilesMustBeEmbeddedResourcesConventionSpecification : SolutionConventionSpecification
    {
        private readonly string _fileExtension;

        private const string FilePattern = @"<(?<BuildAction>\w+) Include=""(?<File>.+\.{0})\"" />";
        private const string EmbeddedResourceBuildAction = "EmbeddedResource";

        public FilesMustBeEmbeddedResourcesConventionSpecification(string fileExtension)
        {
            _fileExtension = fileExtension;
        }

        protected override string FailureMessage
        {
            get
            {
                return "All files with the extension '{0}' within this solution must have their build action set to 'Embedded Resource'".FormatWith(_fileExtension);
            }
        }

        public override ConventionResult IsSatisfiedBy(string solutionRoot)
        {
            var failures = new List<string>();
            var projectFilePaths = Directory.GetFiles(solutionRoot, "*.*proj", SearchOption.AllDirectories);

            foreach (var projectFilePath in projectFilePaths)
            {
                var fileContents = File.ReadAllText(projectFilePath);
                var fileMatches = Regex.Matches(
                    fileContents,
                    string.Format(FilePattern, ExtensionWithoutLeadingPeriod(_fileExtension)),
                    RegexOptions.IgnoreCase
                )
                .Cast<Match>()
                .ToArray();

                var nonEmbeddedFiles = fileMatches.Where(match => !IsFileAnEmbeddedResource(match)).ToArray();

                if (nonEmbeddedFiles.Any())
                {
                    var nonEmbeddedFilePaths = string.Join(
                        Environment.NewLine, 
                        nonEmbeddedFiles.Select(match => "- {0}".FormatWith(BuildFilePath(match, projectFilePath, solutionRoot)))
                    );

                    failures.Add(nonEmbeddedFilePaths);
                }

            }

            if (failures.Any())
            {
                return ConventionResult.NotSatisfied(
                    SolutionConventionResultIdentifier,
                    BuildFailureMessage(failures.Aggregate(string.Empty, (result, input) => result + Environment.NewLine + input).Trim()));
            }

            return ConventionResult.Satisfied(SolutionConventionResultIdentifier);
        }

        private static bool IsFileAnEmbeddedResource(Match match)
        {
            var fileBuildAction = match.Groups["BuildAction"].Value;
            return fileBuildAction.Equals(EmbeddedResourceBuildAction, StringComparison.InvariantCultureIgnoreCase);
        }

        private static string BuildFilePath(Match match, string projectFilePath, string solutionRoot)
        {
            var nonEmbeddFilePathRelativeToProjectFile = match.Groups["File"].Value;
            var projectDirectory = Path.GetDirectoryName(projectFilePath);

            var absolutePathToNonEmbeddFile = Path.Combine(projectDirectory, nonEmbeddFilePathRelativeToProjectFile);
            return absolutePathToNonEmbeddFile.Replace(solutionRoot, "");
        }

        private string ExtensionWithoutLeadingPeriod(string fileExtension)
        {
            return fileExtension
                .TrimStart('*')
                .TrimStart('.');
        }
    }
}
