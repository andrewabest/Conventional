using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Mono.Cecil;

namespace Conventional.Conventions.Cecil
{
    public class ConventionalAssemblyResolver : IAssemblyResolver
    {
        private const string FileSchemePrefix = "file:///";

        public virtual AssemblyDefinition Resolve(string fullName)
        {
            return Resolve(fullName, new ReaderParameters());
        }

        public virtual AssemblyDefinition Resolve(string fullName, ReaderParameters parameters)
        {
            if (fullName == null)
                throw new ArgumentNullException(nameof(fullName));

            return Resolve(AssemblyNameReference.Parse(fullName), parameters);
        }

        public AssemblyDefinition Resolve(AssemblyNameReference name)
        {
            return Resolve(name, new ReaderParameters());
        }

        public AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var assembly = Assembly.Load(name.Name);

            return AssemblyDefinition.ReadAssembly(GetPathToAssembly(assembly), new ReaderParameters { AssemblyResolver = this });
        }

        private static string GetPathToAssembly(Assembly assembly)
        {
            var codebase = assembly.CodeBase.Replace(FileSchemePrefix, string.Empty);

            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) 
                ? codebase 
                : $"/{codebase}";
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}