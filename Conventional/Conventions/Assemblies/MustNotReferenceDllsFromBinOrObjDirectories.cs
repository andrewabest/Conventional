using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Conventional.Conventions.Assemblies
{
    public class MustNotReferenceDllsFromBinOrObjDirectoriesConventionSpecification : AssemblyConventionSpecification
    {
        private const string ObjOrBinPattern = @"^(.*?(obj|bin).*?)$";

        protected override string FailureMessage => "Assembly {0} must not reference Dlls from Bin or Obj directories.";

        protected override ConventionResult IsSatisfiedByLegacyCsprojFormat(string assemblyName, XDocument projectDocument)
        {
            // NOTE: legacy Csproj refs follow the same format as 2017 Csproj

            return IsSatisfiedBy(assemblyName, projectDocument);
        }

        protected override ConventionResult IsSatisfiedBy(string assemblyName, XDocument projectDocument)
        {
            // NOTE: The nesting we are looking for is Project > ItemGroup > Reference > HintPath
            // NOTE: <ItemGroup>
            // NOTE:     <Reference Include = "Mono.Cecil, Version=0.9.5.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756, processorArchitecture=MSIL">
            // NOTE:     <SpecificVersion> False </SpecificVersion>
            // NOTE:     <HintPath>..\packages\Mono.Cecil.0.9.5.4\lib\net40\Mono.Cecil.dll </HintPath>
            // NOTE:     </Reference>

            var references =
                projectDocument.Elements()
                    .Single(x => x.Name.LocalName == "Project")
                    .Elements()
                    .Where(x => x.Name.LocalName == "ItemGroup")
                    .SelectMany(x => x.Elements().Where(e => e.Name.LocalName == "Reference"));

            var failures =
                references
                    .Where(x => x.Elements().Any(e => e.Name.LocalName == "HintPath" && Regex.IsMatch(e.Value, ObjOrBinPattern)))
                    .Select(x =>
                        new Failure(
                            x.Attributes().Single(a => a.Name.LocalName == "Include").Value,
                            x.Elements().Single(e => e.Name.LocalName == "HintPath").Value))
                    .ToArray();

            if (failures.Any())
            {
                var failureText =
                    FailureMessage.FormatWith(assemblyName) +
                    Environment.NewLine +
                    failures.Aggregate(string.Empty,
                        (s, t) => s + "\t" + t.Reference + " " + "(" + t.Location + ")" + Environment.NewLine);

                return ConventionResult.NotSatisfied(assemblyName, failureText);
            }

            return ConventionResult.Satisfied(assemblyName);
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