using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Xml.Linq;

namespace Conventional
{
    public static class XmlExpando
    {
        public static dynamic Expand(this XDocument xDocument)
        {
            if (xDocument == null)
            {
                throw new ArgumentNullException("xDocument");
            }

            if (xDocument.Root == null)
            {
                throw new ArgumentException("Document contains no elements", "xDocument");
            }

            var xmlExpando = new ExpandoObject();
            ((IDictionary<string, object>)xmlExpando)[xDocument.Root.Name.LocalName] = new ExpandoObject();
            ProcessChildren(((IDictionary<string, object>)xmlExpando)[xDocument.Root.Name.LocalName], xDocument.Root, new Dictionary<string, int>());

            return xmlExpando;
        }

        private static void ProcessChildren(dynamic xmlExpando, XElement xElement, IDictionary<string, int> duplicateKeyTracker)
        {
            if (xElement.HasAttributes)
            {
                xmlExpando.Attributes = xElement.Attributes().Select(attribute => new { attribute.Name.LocalName, attribute.Value }).ToArray();
            }
            if (xElement.HasElements)
            {
                var indexableXmlExpando = (IDictionary<string, object>)xmlExpando;
                foreach (var childXElement in xElement.Elements())
                {
                    var elementKey = BuildName(indexableXmlExpando, childXElement.Name.LocalName, duplicateKeyTracker);
                    indexableXmlExpando[elementKey] = new ExpandoObject();
                    ProcessChildren(indexableXmlExpando[elementKey], childXElement, duplicateKeyTracker);
                }
            }
            else
            {
                xmlExpando.Value = xElement.Value;
            }
        }

        private static string BuildName(IDictionary<string, object> indexableXmlExpando, string childElementKey, IDictionary<string, int> duplicateKeyTracker)
        {
            if (!indexableXmlExpando.ContainsKey(childElementKey))
            {
                return childElementKey;
            }

            var hasExistingDuplicates = duplicateKeyTracker.TryGetValue(childElementKey, out var duplicateCount);
            if (hasExistingDuplicates)
            {
                duplicateCount = ++duplicateKeyTracker[childElementKey];
            }
            else
            {
                duplicateCount = 1;
                duplicateKeyTracker.Add(childElementKey, duplicateCount);
            }

            return childElementKey + duplicateCount;
        }
    }
}