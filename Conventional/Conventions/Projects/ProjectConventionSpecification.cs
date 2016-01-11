using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Conventional.Conventions.Solution;
using Conventional.Extensions;

namespace Conventional.Conventions.Projects
{
    public abstract class ProjectConventionSpecification : IAssemblyConventionSpecification
    {
        protected string ProjectFilePath;
        protected string ProjectFolder { get { return new FileInfo(ProjectFilePath).DirectoryName;} }
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

            return IsSatisfiedByInternal(projectDocument.Expand().Project.PropertyGroup.AssemblyName.Value,
                projectDocument);
        }

        public ConventionResult IsSatisfiedBy(Assembly assembly)
        {
            ProjectFilePath = assembly.ResolveProjectFilePath();

            return IsSatisfiedBy(ProjectFilePath);
        }

        protected abstract ConventionResult IsSatisfiedByInternal(string assemblyName, XDocument projectDocument);
    }
}