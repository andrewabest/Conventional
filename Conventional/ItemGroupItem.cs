using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Conventional
{
    internal class ItemGroupItem
    {
        private readonly string _type;
        private readonly string _include;

        public ItemGroupItem(XElement itemGroupItemAsXml)
        {
            _type = itemGroupItemAsXml.Name.LocalName;

            var includeAttribute = itemGroupItemAsXml.Attributes().SingleOrDefault(a => a.Name.LocalName == "Include");
            _include = includeAttribute == null ? "" : includeAttribute.Value;
        }

        public bool MatchesPatternAndIsNotAnEmbeddedResourceOrReference(Regex fileMatchRegex)
        {
            if (_type == "EmbeddedResource")
            {
                return false;
            }

            if (_type == "Reference")
            {
                return false;
            }

            return fileMatchRegex.IsMatch(_include);
        }

        public bool MatchesAbsolutePath(string path)
        {
            return _include == path;
        }

        public override string ToString()
        {
            return "{0} [type={1}]".FormatWith(_include, _type);
        }

        public static IEnumerable<ItemGroupItem> FromProjectDocument(XDocument projectDocument)
        {
            return projectDocument
                .Elements().Single(x => x.Name.LocalName == "Project")
                .Elements().Where(x => x.Name.LocalName == "ItemGroup")
                .SelectMany(x => x.Elements().Select(element => new ItemGroupItem(element)));
        }
    }
}