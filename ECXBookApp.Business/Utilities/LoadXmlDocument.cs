using System;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

namespace ECXBookApp.Business.Utilities
{
    public class LoadXmlDocument
    {
        public string LoadXMLObject(string filepath)
        {
            var xmlObj = string.Empty;
            var readxml = File.ReadAllText(filepath);

            if (readxml.StartsWith("<"))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(readxml);
                xmlObj = JsonConvert.SerializeXmlNode(doc);
            }

            return xmlObj;
        }
    }
}
