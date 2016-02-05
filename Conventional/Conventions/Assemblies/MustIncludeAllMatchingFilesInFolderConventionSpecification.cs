using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Conventional.Conventions.Assemblies
{
    public class MustIncludeAllMatchingFilesInFolderConventionSpecification : AssemblyConventionSpecification
    {
        private readonly string _pattern;

        public MustIncludeAllMatchingFilesInFolderConventionSpecification(string pattern)
        {
            _pattern = pattern;
        }

        protected override string FailureMessage
        {
            get { return "All files matching '{0}' within project folder for '{1}' must be included in the project."; }
        }

        protected override ConventionResult IsSatisfiedByInternal(string assemblyName, XDocument projectDocument)
        {
            var files = AllMatchingFilesInFolder(ProjectFolder, _pattern);
            var failures = files.Where(file =>
                !ItemGroupItem.FromProjectDocument(projectDocument).Any(igi => igi.MatchesAbsolutePath(file.Replace(this.ProjectFolder, "").TrimStart('\\'))))
                .ToArray();

            if (failures.Any())
            {
                var failureText = FailureMessage.FormatWith(_pattern, ProjectFilePath) +
                                  Environment.NewLine +
                                  string.Join(Environment.NewLine, failures.Select(igi => "- " + igi.ToString()));
                return ConventionResult.NotSatisfied(assemblyName, failureText);
            }

            return ConventionResult.Satisfied(assemblyName);
        }

        private static IEnumerable<string> AllMatchingFilesInFolder(string path, string pattern)
        {
            return
                Directory.GetFiles(path, pattern)
                    .Union(
                        Directory.GetDirectories(path)
                            .SelectMany(subFolder => AllMatchingFilesInFolder(subFolder, pattern)));
        }
    }
}