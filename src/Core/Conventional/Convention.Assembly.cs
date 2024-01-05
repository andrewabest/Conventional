﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Conventional.Conventions.Assemblies;

namespace Conventional
{
    public static partial class Convention
    {
        [Obsolete("Replace with MustNotReferenceDllsFromTransientOrSdkDirectories")]
        public static MustNotReferenceDllsFromTransientOrSdkDirectoriesConventionSpecification MustNotReferenceDllsFromBinOrObjDirectories =>
            new MustNotReferenceDllsFromTransientOrSdkDirectoriesConventionSpecification();

        public static MustNotReferenceDllsFromTransientOrSdkDirectoriesConventionSpecification MustNotReferenceDllsFromTransientOrSdkDirectories =>
            new MustNotReferenceDllsFromTransientOrSdkDirectoriesConventionSpecification();

        public static MustHaveAllFilesBeResourcesConventionSpecification MustHaveFilesBeResources(string fileExtension)
        {
            return new MustHaveAllFilesBeResourcesConventionSpecification(fileExtension);
        }

        public static MustHaveAllFilesBeResourcesConventionSpecification MustHaveFilesBeResources(Regex fileMatchRegex)
        {
            return new MustHaveAllFilesBeResourcesConventionSpecification(fileMatchRegex);
        }

        public static MustHaveAllFilesBeEmbeddedResourcesConventionSpecification MustHaveFilesBeEmbeddedResources(
            string fileExtension)
        {
            return new MustHaveAllFilesBeEmbeddedResourcesConventionSpecification(fileExtension);
        }

        public static MustHaveAllFilesBeEmbeddedResourcesConventionSpecification MustHaveFilesBeEmbeddedResources(
            Regex fileMatchRegex)
        {
            return new MustHaveAllFilesBeEmbeddedResourcesConventionSpecification(fileMatchRegex);
        }

        public static MustHaveFilesBeContentConventionSpecification MustHaveFilesBeContent(string fileExtension)
        {
            return new MustHaveFilesBeContentConventionSpecification(fileExtension);
        }

        public static MustHaveFilesBeContentConventionSpecification MustHaveFilesBeContent(Regex fileMatchRegex)
        {
            return new MustHaveFilesBeContentConventionSpecification(fileMatchRegex);
        }

        /// <summary>
        /// Requires all files which match a certain pattern to be included in the project file. You might use this if you have some
        /// developers using a different editor, and want to ensure they add all new files to the project.
        /// </summary>
        /// <param name="filePattern">Any pattern which can be used with Directory.GetFiles</param>
        public static MustIncludeAllMatchingFilesInFolderConventionSpecification MustIncludeAllMatchingFilesInFolder(
            string filePattern)
        {
            return new MustIncludeAllMatchingFilesInFolderConventionSpecification(filePattern);
        }

        /// <summary>
        /// Requires all files which match a certain pattern to be included in the project file. You might use this if you have some
        /// developers using a different editor, and want to ensure they add all new files to the project.
        /// </summary>
        /// <param name="filePattern">Any pattern which can be used with Directory.GetFiles</param>
        /// <param name="subfolder">A subfolder path to limit the convention to</param>
        public static MustIncludeAllMatchingFilesInFolderConventionSpecification MustIncludeAllMatchingFilesInFolder(
            string filePattern, string subfolder)
        {
            return new MustIncludeAllMatchingFilesInFolderConventionSpecification(filePattern, subfolder);
        }

        /// <summary>
        /// Require this assembly to be included (by name) in a list of assemblies (e.g. "TestAssemblies")
        /// </summary>
        /// <param name="assemblies">The "haystack" set of assemblies that the "needle" assembly must appear in</param>
        /// <param name="setName">A friendly name for the assembly set</param>
        /// <remarks>Probably only useful when using <see cref="AssemblySpecimen"/> i.e. <see cref="TheAssembly"/> / <see cref="AllAssemblies"/></remarks>
        public static MustBeIncludedInSetOfAssembliesConventionSpecification MustBeIncludedInSetOfAssemblies(
            IEnumerable<Assembly> assemblies, string setName)
        {
            return new MustBeIncludedInSetOfAssembliesConventionSpecification(assemblies, setName);
        }
    }
}