using BoseSoundTouchApp.Models;
using System.Collections.Generic;
using System.Net;
using System.Xml;

namespace BoseSoundTouchApp.Services
{
    public class POST
    {
        #region Members
        readonly IPAddress m_ipAddress;
        #endregion Members

        #region SubTypes
        public class Node
        {
            public Node(string name, string value)
            {
                Name = name;
                Value = value;
            }

            public string Name
            {
                get;
                private set;
            }

            public string Value
            {
                get;
                private set;
            }

            public Dictionary<string, string> Attributes
            {
                get;
                set;
            }

            public List<Node> SubNodes
            {
                get;
                set;
            }
        }
        #endregion SubTypes

        public POST(IPhysicalData physicalData)
        {
            m_ipAddress = physicalData.Address;
        }

        public string Post(Node node)
        {
            var serviceName = GetType().Name.Substring("POST_".Length);
            string url = "http://" + m_ipAddress.ToString() + ":8090" + "/" + serviceName;
            XmlDocument doc = new XmlDocument();
            var declartion = doc.CreateXmlDeclaration("1.0", string.Empty, string.Empty);
            doc.InsertBefore(declartion, doc.DocumentElement);
            AddNode(doc, doc, node);
            return PostXmlDataAsync(url, doc);
        }

        private void AddNode(XmlDocument doc, XmlNode node, Node data)
        {
            var element = doc.CreateElement(data.Name);
            var text = doc.CreateTextNode(data.Value);
            element.AppendChild(text);
            if (data.Attributes != null)
            {
                foreach (var attr in data.Attributes)
                {
                    element.SetAttribute(attr.Key, attr.Value);
                }
            }

            if (data.SubNodes != null)
            {
                foreach(var sub in data.SubNodes)
                {
                    AddNode(doc, element, sub);
                }
            }

            doc.AppendChild(element);
        }

        private string PostXmlDataAsync(string url, XmlDocument doc)
        {
            using (WebClient client = new WebClient())
            {
                string result = client.UploadString(address: url, data: doc.OuterXml);
                return result;
            }
        }
    }
}
