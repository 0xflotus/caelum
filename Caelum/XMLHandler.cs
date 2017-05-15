using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Caelum
{
    public static class XMLHandler
    {
        public static String FILENAME = "settings.xml";

        public static void WriteXML(String path, Dictionary<String, bool> val)
        {
            XmlWriter writer = XmlWriter.Create(path);
            writer.WriteStartDocument();
            writer.WriteStartElement("states");
            val.ToList().ForEach(kv => WriteElement(writer, kv.Key, kv.Value));
            writer.WriteEndDocument();
            writer.Close();
        }

        private static void WriteElement(XmlWriter wr, String element, bool attributeSet)
        {
            wr.WriteStartElement(element);
            wr.WriteAttributeString("checked", attributeSet ? "YES" : "NO");
            wr.WriteEndElement();
        }

        public static bool IsChecked(String filepath, String xpath)
        {
            XmlDocument doc = new XmlDocument();
            try { doc.Load(filepath); }
            catch (Exception ex) { Funktionen.actions["Log"](ex.Message); }
            return doc.SelectSingleNode(xpath) != null ? doc.SelectSingleNode(xpath).Attributes["checked"].Value == "YES" : false;
        }
    }
}