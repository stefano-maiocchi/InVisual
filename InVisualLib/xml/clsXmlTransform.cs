using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.IO;

namespace InVisualLib.xml
{
    public static class clsXmlTransform
    {

        public static void transformAndSaveHtml(string pathXml, string pathXslt, string pathHtml)
        {
            XslCompiledTransform xslTrans = new XslCompiledTransform();
            xslTrans.Load(pathXslt);
            xslTrans.Transform(pathXml, pathHtml);            
        }

        public static string getHtml(string pathXml, string pathXslt)
        {
            XslCompiledTransform xslTrans = new XslCompiledTransform();            
            string html = "";
            
            xslTrans.Load(pathXslt);        

            using (StringWriter sw = new StringWriter())
            {
                xslTrans.Transform(pathXml, null, sw);
                return sw.ToString();
            }                      

        }

        
    }
}
