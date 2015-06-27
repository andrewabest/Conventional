using System;
using System.Reflection;
using System.Xml.Linq;
using Conventional.Conventions.Solution;
using Mono.Cecil;
using Mono.Cecil.Cil;

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

            return IsSatisfiedByInternal(projectDocument.Expand().Project.PropertyGroup.AssemblyName, projectDocument);
        }

        protected abstract ConventionResult IsSatisfiedByInternal(string assemblyName, XDocument projectDocument);

        static string ResolveProjectFilePath(Assembly assembly)
        {
            var readerParameters = new ReaderParameters { ReadSymbols = true, ReadingMode = ReadingMode.Deferred };
            var definition = AssemblyDefinition.ReadAssembly(assembly.Location, readerParameters);
            var methodDefinition = GetMethodWithBody(definition);
            var document = GetMethodDocument(methodDefinition);
            return document.Url;
        }

        static Document GetMethodDocument(MethodDefinition methodDefinition)
        {
            var instruction = methodDefinition.Body.Instructions[0];
            var document = instruction.SequencePoint.Document;
            return document;
        }

        static MethodDefinition GetMethodWithBody(AssemblyDefinition definition)
        {
            foreach (var type in definition.MainModule.Types)
            {
                if (type.HasMethods == false) continue;

                foreach (var method in type.Methods)
                {
                    if (method.HasBody)
                    {
                        return method;
                    }
                }
            }

            throw new InvalidOperationException("Couldn't find any type with methods with a body");
        }
    }
}