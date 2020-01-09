using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Conventional.Conventions.Assemblies;

namespace Conventional
{
    internal class ItemGroupItem
    {
        private readonly string _type;
        private readonly bool _preserveNewest;

        protected ItemGroupItem(XElement itemGroupItemAsXml, bool isLegacyCsprojFormat)
        {
            _type = itemGroupItemAsXml.Name.LocalName;

            var includeAttribute = itemGroupItemAsXml.Attributes().SingleOrDefault(a => a.Name.LocalName == "Include");
            Include = includeAttribute?.Value ?? "";

            var updateAttribute = itemGroupItemAsXml.Attributes().SingleOrDefault(a => a.Name.LocalName == "Update");
            Update = updateAttribute?.Value ?? "";

            var copyToOutputDirectoryName =
                isLegacyCsprojFormat
                    ? XName.Get("CopyToOutputDirectory", LegacyCsprojConstants.Namespace)
                    : XName.Get("CopyToOutputDirectory");

            var copyToOutputDirectory =
                itemGroupItemAsXml.Descendants(copyToOutputDirectoryName).FirstOrDefault();

            _preserveNewest = copyToOutputDirectory != null && copyToOutputDirectory.Value == "PreserveNewest";
        }

        public string Include { get; }

        public string Update { get; }

        public bool MatchesPatternAndIsNotAnResourceOrReference(Regex fileMatchRegex)
        {
            return _type != "Resource" && fileMatchRegex.IsMatch(Include);
        }

        public bool MatchesPatternAndIsNotAnEmbeddedResource(Regex fileMatchRegex)
        {
            return _type != "EmbeddedResource" && fileMatchRegex.IsMatch(Include);
        }

        public bool MatchesPatternAndIsNotAnEmbeddedResourceOrReference(Regex fileMatchRegex)
        {
            return _type != "Reference" && MatchesPatternAndIsNotAnEmbeddedResource(fileMatchRegex);
        }

        public bool MatchesPatternAndIsNotContentCopyNewest(Regex fileMatchRegex)
        {
            if (_type == "Content" && _preserveNewest)
            {
                return false;
            }

            return fileMatchRegex.IsMatch(Include);
        }

        public bool MatchesAbsolutePath(string path)
        {
            return Include == path;
        }

        public override string ToString()
        {
            return "{0} [type={1}]".FormatWith(Include, _type);
        }

        public static IEnumerable<ItemGroupItem> FromProjectDocument(XDocument projectDocument)
        {
            return projectDocument
                .Elements().Single(x => x.Name.LocalName == "Project")
                .Elements().Where(x => x.Name.LocalName == "ItemGroup")
                .SelectMany(x => x.Elements().Select(element => new ItemGroupItem(element, projectDocument.IsLegacyCsprojFormat())));
        }
    }
}