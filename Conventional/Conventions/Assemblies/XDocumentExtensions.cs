using System.Xml.Linq;

namespace Conventional.Conventions.Assemblies
{
    public static class XDocumentExtensions
    {
        public static bool IsLegacyCsprojFormat(this XDocument document)
        {
            return document.Element(XName.Get("Project", LegacyCsprojConstants.Namespace)) != null;
        }
    }
}