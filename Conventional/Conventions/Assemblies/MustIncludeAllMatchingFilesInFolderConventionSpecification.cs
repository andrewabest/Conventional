using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Conventional.Extensions;

namespace Conventional.Conventions.Assemblies
{
    public class MustIncludeAllMatchingFilesInFolderConventionSpecification : AssemblyConventionSpecification
    {
        private readonly string _pattern;
        private readonly string _subfolder = "";
        private string[] _excludedExtensions = new string[0];
        private string ConventionTargetFolder => Path.Combine(ProjectFolder, _subfolder);

        /// <summary>
        /// Requires all files which match a certain pattern to be included in the project file. You might use this if you have some
        /// developers using a different editor, and want to ensure they add all new files to the project.
        /// </summary>
        /// <param name="pattern">Any pattern which can be used with Directory.GetFiles</param>
        public MustIncludeAllMatchingFilesInFolderConventionSpecification(string pattern)
        {
            _pattern = pattern;
        }

        /// <inheritdoc />
        /// <param name="subfolder">A subfolder path to limit the convention to</param>
        public MustIncludeAllMatchingFilesInFolderConventionSpecification(string pattern, string subfolder) : this(pattern)
        {
            _subfolder = subfolder;
        }

        protected override string FailureMessage => "All files matching '{0}' within '{1}' must be included in the project.";

        protected override ConventionResult IsSatisfiedByLegacyCsprojFormat(string assemblyName, XDocument projectDocument)
        {
            var files = AllMatchingFilesInFolder(ConventionTargetFolder);

            var failures = files.Where(file =>
                ItemGroupItem.FromProjectDocument(projectDocument).None(igi => igi.MatchesAbsolutePath(file.Replace(this.ProjectFolder, "").TrimStart('\\'))))
                .ToArray();

            if (failures.Any())
            {
                var failureText = FailureMessage.FormatWith(_pattern, ConventionTargetFolder) +
                                  Environment.NewLine +
                                  string.Join(Environment.NewLine, failures.Select(igi => "- " + igi.ToString()));

                return ConventionResult.NotSatisfied(assemblyName, failureText);
            }

            return ConventionResult.Satisfied(assemblyName);
        }

        protected override ConventionResult IsSatisfiedBy(string assemblyName, XDocument projectDocument)
        {
            // NOTE: File inclusion rules for 2017 Csproj
            // NOTE: Element | Include glob | Exclude glob | Remove glob
            // NOTE: Compile | **/*.cs (or other language extensions) | **/*.user; **/*.*proj; **/*.sln; **/*.vssscc | N/A
            // NOTE: EmbeddedResource	| **/*.resx | **/*.user; **/*.*proj; **/*.sln; **/*.vssscc | N/A
            // NOTE: None **/* | **/*.user; **/*.*proj; **/*.sln; **/*.vssscc | - **/*.cs; **/*.resx

            // TODO: Less Naive implementation for 2017 Csproj users

            var nonDefaultIncludeRulesInUse = 
                projectDocument.Elements().Where(x => x.Name.LocalName == "PropertyGroup")
                    .Any(x => x.Elements(XName.Get("EnableDefaultCompileItems")) != null);

            return nonDefaultIncludeRulesInUse
                ? IsSatisfiedByLegacyCsprojFormat(assemblyName, projectDocument)
                : ConventionResult.Satisfied(assemblyName);
        }

        private static readonly string[] ProjectOutputFolders = {"obj", "bin"};
        private IEnumerable<string> AllMatchingFilesInFolder(string path, bool skipProjectOutputFolders = true)
        {
            if (skipProjectOutputFolders &&
                ProjectOutputFolders.Any(p => string.Equals(Path.Combine(ProjectFolder, p), path, StringComparison.CurrentCultureIgnoreCase)))
            {
                return new string[0];
            }

            return
                DirectoryEx
                    .GetFilesExceptOutput(path, _pattern)
                    .Where(p => _excludedExtensions.Contains(new FileInfo(p).Extension.ToLower()) == false);
        }

        public void AddFilenameExtensionExclusions(params string[] extensions)
        {
            var extensionsWithPeriod = extensions.Select(e => (e.StartsWith(".") ? e : "." + e).ToLower()).ToArray();
            _excludedExtensions = _excludedExtensions?.Union(extensionsWithPeriod).ToArray() ?? extensionsWithPeriod;
        }
    }
}