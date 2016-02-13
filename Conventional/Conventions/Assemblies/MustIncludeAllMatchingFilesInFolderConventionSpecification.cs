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
        private readonly string _subfolder = "";
        private string[] _excludedExtensions = new string[0];
        private string ConventionTargetFolder
        {
            get
            {
                return Path.Combine(ProjectFolder, _subfolder);
            }
        }

        /// <summary>
        /// Requires all files which match a certain pattern to be included in the project file. You might use this if you have some
        /// developers using a different editor, and want to ensure they add all new files to the project.
        /// </summary>
        /// <param name="pattern">Any pattern which can be used with Directory.GetFiles</param>
        public MustIncludeAllMatchingFilesInFolderConventionSpecification(string pattern)
        {
            _pattern = pattern;
        }

        /// <summary>
        /// Requires all files which match a certain pattern to be included in the project file. You might use this if you have some
        /// developers using a different editor, and want to ensure they add all new files to the project.
        /// </summary>
        /// <param name="pattern">Any pattern which can be used with Directory.GetFiles</param>
        /// <param name="subfolder">A subfolder path to limit the convention to</param>
        public MustIncludeAllMatchingFilesInFolderConventionSpecification(string pattern, string subfolder) : this(pattern)
        {
            _subfolder = subfolder;
        }

        protected override string FailureMessage
        {
            get { return "All files matching '{0}' within '{1}' must be included in the project."; }
        }

        protected override ConventionResult IsSatisfiedByInternal(string assemblyName, XDocument projectDocument)
        {
            var files = AllMatchingFilesInFolder(ConventionTargetFolder);
            var failures = files.Where(file =>
                !ItemGroupItem.FromProjectDocument(projectDocument).Any(igi => igi.MatchesAbsolutePath(file.Replace(this.ProjectFolder, "").TrimStart('\\'))))
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

        private static readonly string[] ProjectOutputFolders = {"obj", "bin"};
        private IEnumerable<string> AllMatchingFilesInFolder(string path, bool skipProjectOutputFolders = true)
        {
            if (skipProjectOutputFolders && 
                ProjectOutputFolders.Any(p => Path.Combine(ProjectFolder, p).ToLower() == path.ToLower()))
                return new string[0];
            return
                Directory.GetFiles(path, _pattern).Where(p => !_excludedExtensions.Contains(new FileInfo(p).Extension.ToLower()))
                    .Union(
                        Directory.GetDirectories(path)
                            .SelectMany(subFolder => AllMatchingFilesInFolder(subFolder)));
        }

        public void AddFilenameExtensionExclusions(params string[] extensions)
        {
            var extensionsWithPeriod = extensions.Select(e => (e.StartsWith(".") ? e : "." + e).ToLower()).ToArray();
            _excludedExtensions = _excludedExtensions?.Union(extensionsWithPeriod).ToArray() ?? extensionsWithPeriod;
        }
    }
}