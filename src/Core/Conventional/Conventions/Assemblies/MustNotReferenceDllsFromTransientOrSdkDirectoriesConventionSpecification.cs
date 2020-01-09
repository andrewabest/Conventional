using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Conventional.Conventions.Assemblies
{
    public class MustNotReferenceDllsFromTransientOrSdkDirectoriesConventionSpecification : AssemblyConventionSpecification
    {
        private const string FindPattern = @"^(.*?(obj|bin|NuGetFallbackFolder|Reference Assemblies).*?)$";

        protected override string FailureMessage => "Assembly {0} must not reference Dlls from transient (bin, obj) directories, package caches, or reference assemblies.";

        protected override ConventionResult IsSatisfiedByLegacyCsprojFormat(string assemblyName, XDocument projectDocument)
        {
            return IsSatisfiedBy(assemblyName, projectDocument);
        }

        protected override ConventionResult IsSatisfiedBy(string assemblyName, XDocument projectDocument)
        {
            // NOTE: The nesting we are looking for is Project > ItemGroup > Reference > HintPath
            // NOTE: <ItemGroup>
            // NOTE:     <Reference Include = "Mono.Cecil, Version=0.9.5.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756, processorArchitecture=MSIL">
            // NOTE:     <SpecificVersion> False </SpecificVersion>
            // NOTE:     <HintPath>..\packages\Mono.Cecil.0.9.5.4\lib\net40\Mono.Cecil.dll </HintPath>
            // NOTE: </Reference>
            // NOTE:
            // NOTE: and
            // NOTE:
            // NOTE: <ItemGroup>
            // NOTE:   <Reference Include="Microsoft.AspNetCore.Mvc.Core">
            // NOTE:     <HintPath>..\..\..\..\..\Program Files\dotnet\sdk\NuGetFallbackFolder\microsoft.aspnetcore.mvc.core\2.0.3\lib\netstandard2.0\Microsoft.AspNetCore.Mvc.Core.dll</HintPath>
            // NOTE:   </Reference>
            // NOTE: </ItemGroup>

            var references =
                projectDocument.Elements()
                    .Single(x => x.Name.LocalName == "Project")
                    .Elements()
                    .Where(x => x.Name.LocalName == "ItemGroup")
                    .SelectMany(x => x.Elements().Where(e => e.Name.LocalName == "Reference"));

            var failures = references
                .Where(x => x.Elements().Any(e => e.Name.LocalName == "HintPath" && Regex.IsMatch(e.Value, FindPattern)))
                .Select(x => new Failure(
                    x.Attributes().Single(a => a.Name.LocalName == "Include").Value,
                    x.Elements().Single(e => e.Name.LocalName == "HintPath").Value))
                .ToArray();

            if (!failures.Any())
                return ConventionResult.Satisfied(assemblyName);

            var failureText = $"{FailureMessage.FormatWith(assemblyName)}{Environment.NewLine}{failures.Aggregate(string.Empty, (s, t) => $"{s}\t{t.Reference} ({t.Location}){Environment.NewLine}")}";
            return ConventionResult.NotSatisfied(assemblyName, failureText);
        }

        private class Failure
        {
            public Failure(string reference, string location)
            {
                Reference = reference;
                Location = location;
            }

            public string Reference { get; }
            public string Location { get; }
        }
    }
}