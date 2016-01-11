using System;
using System.Reflection;
using System.Xml.Linq;
using Conventional.Conventions.Solution;
using Conventional.Extensions;

namespace Conventional.Conventions.Assemblies
{
    public abstract class AssemblyConventionSpecification : IAssemblyConventionSpecification
    {
        protected abstract string FailureMessage { get; }

        protected string BuildFailureMessage(string details)
        {
            return FailureMessage +
                   Environment.NewLine +
                   details;
        }

        public ConventionResult IsSatisfiedBy(string projectFilePath)
        {
            var projectDocument = XDocument.Load(projectFilePath);

            return IsSatisfiedByInternal(projectDocument.Expand().Project.PropertyGroup.AssemblyName.Value,
                projectDocument);
        }

        public ConventionResult IsSatisfiedBy(Assembly assembly)
        {
            var projectFilePath = assembly.ResolveProjectFilePath();

            return IsSatisfiedBy(projectFilePath);
        }

        protected abstract ConventionResult IsSatisfiedByInternal(string assemblyName, XDocument projectDocument);
    }
}