using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCenter.Eyes.Archive2Xml
{
    public abstract class ArchivedObject
    {
        protected XmlDocument Document { get; private set; }
        protected XmlElement Root { get; private set; }

        public ArchivedObject(string root)
        {
            this.Document = new XmlDocument();
            var dec = this.Document.CreateXmlDeclaration("1.0", "UTF-8", string.Empty);
            this.Document.AppendChild(dec);

            this.Root = this.Document.CreateElement(root);
            this.Document.AppendChild(this.Root);
        }

        protected XmlElement AddTextElement(XmlElement parent, string name, string value)
        {
            var element = this.Document.CreateElement(name);
            element.InnerText = value;

            parent.AppendChild(element);

            return element;
        }

        protected XmlElement AddCDATAElement(XmlElement parent, string name, string value)
        {
            var element = this.Document.CreateElement(name);
            element.InnerXml = string.Format("<![CDATA[{0}]]>", value);

            parent.AppendChild(element);

            return element;
        }

        public virtual void Save(string path)
        {
            this.Document.Save(path);
        }

        public virtual void Save()
        {
            this.Document.Save(this.FileName);
        }

        public abstract string FileName
        {
            get;
        }
    }
}
