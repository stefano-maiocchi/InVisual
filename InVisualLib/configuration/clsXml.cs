using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace InVisualLib.configuration
{
    public class clsXml
    {
      
        public static string getValue(string filename, string mode, string id)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename));

            string temp = "";
            string xPath = "//root/" + mode + "/par[@id='" + id + "']";

            XmlNode nod = doc.SelectSingleNode(xPath);
            if (nod != null) temp = nod.Attributes["value"].Value;
            doc = null;
            return temp;
        }

        public static string getValue(string filename, string mode, string id, string defaultValue)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename));

            string temp = "";
            string xPath = "//root/" + mode + "/par[@id='" + id + "']";

            XmlNode nod = doc.SelectSingleNode(xPath);
            if (nod != null) temp = nod.Attributes["value"].Value;
            doc = null;
            return temp.Length == 0 ? defaultValue : temp;
        }

        public static void setValue(string filename, string mode, string id, string value)
        {
            XmlDocument doc = new XmlDocument();
            
            doc.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename));
            string xPath = "//" + mode + "/par[@id='" + id + "']";

            XmlNode nod = doc.SelectSingleNode(xPath);
            if (nod != null) nod.Attributes["value"].Value = value;
            doc.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename));

            doc = null;
        }

    }
}
