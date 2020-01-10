using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Conventional.Extensions;
using Microsoft.CSharp.RuntimeBinder;

namespace Conventional.Conventions.Assemblies
{
    public abstract class AssemblyConventionSpecification : IAssemblyConventionSpecification
    {
        protected string ProjectFilePath;
        protected string ProjectFolder => new FileInfo(ProjectFilePath).DirectoryName;
        protected abstract string FailureMessage { get; }

        protected string BuildFailureMessage(string details)
        {
            return FailureMessage +
                   Environment.NewLine +
                   details;
        }

        public ConventionResult IsSatisfiedBy(string projectFilePath)
        {
            ProjectFilePath = projectFilePath;
            var projectDocument = XDocument.Load(ProjectFilePath);

            string assemblyName;
            try
            {
                assemblyName = projectDocument.Expand().Project.PropertyGroup.AssemblyName.Value;
            }
            catch (RuntimeBinderException)
            {
                assemblyName =
                    projectFilePath.Substring(
                        projectFilePath.LastIndexOf(Path.DirectorySeparatorChar) + 1,
                        projectFilePath.LastIndexOf(".", StringComparison.Ordinal) - projectFilePath.LastIndexOf(Path.DirectorySeparatorChar) - 1);
            }

            return
                projectDocument.IsLegacyCsprojFormat()
                    ? IsSatisfiedByLegacyCsprojFormat(assemblyName, projectDocument)
                    : IsSatisfiedBy(assemblyName, projectDocument);
        }

        public ConventionResult IsSatisfiedBy(Assembly assembly)
        {
            ProjectFilePath = assembly.ResolveProjectFilePath();

            return IsSatisfiedBy(ProjectFilePath);
        }

        protected abstract ConventionResult IsSatisfiedByLegacyCsprojFormat(string assemblyName, XDocument projectDocument);

        protected abstract ConventionResult IsSatisfiedBy(string assemblyName, XDocument projectDocument);
    }
}