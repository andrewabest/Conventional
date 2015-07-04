using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Conventional.Conventions.Solution;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Pdb;

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

        public ConventionResult IsSatisfiedBy(Assembly assembly)
        {
            var projectPath = ResolveProjectFilePath(assembly);

            var projectDocument = XDocument.Load(projectPath);

            return IsSatisfiedByInternal(projectDocument.Expand().Project.PropertyGroup.AssemblyName.Value, projectDocument);
        }

        protected abstract ConventionResult IsSatisfiedByInternal(string assemblyName, XDocument projectDocument);

        static string ResolveProjectFilePath(Assembly assembly)
        {
            var projectFile = assembly.GetName().Name + ".csproj";

            var candidates = Directory.GetFiles(KnownPaths.SolutionRoot, "*" + projectFile, SearchOption.AllDirectories);

            if (candidates.Any() == false)
            {
                throw new ConventionException("Could not locate a project file with the name {0}".FormatWith(projectFile));
            }

            if (candidates.Length > 1)
            {
                throw new ConventionException("More than one project file was located with the name {0}".FormatWith(projectFile));
            }

            return candidates[0];
        }
    }
}