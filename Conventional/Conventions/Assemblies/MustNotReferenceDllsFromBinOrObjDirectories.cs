using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Conventional.Conventions.Assemblies
{
    public class MustNotReferenceDllsFromBinOrObjDirectoriesConventionSpecification : AssemblyConventionSpecification
    {
        private const string ObjOrBinPattern = @"^(.*?(obj|bin).*?)$";

        // NOTE: The nesting we are looking for is Project > ItemGroup > Reference > HintPath
        protected override string FailureMessage
        {
            get { return "Assembly {0} must not reference Dlls from Bin or Obj directories."; }
        }

        protected override ConventionResult IsSatisfiedByInternal(string assemblyName, XDocument projectDocument)
        {
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

            public string Reference { get; set; }
            public string Location { get; set; }
        }
    }
}