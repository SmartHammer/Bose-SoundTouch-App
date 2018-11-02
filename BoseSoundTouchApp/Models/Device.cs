using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace BoseSoundTouchApp.Models
{
    public class Device : Bases.NotiferClass, IDevice
    {
        #region Members
        #endregion Members

        public Device(IPhysicalData physicalData, XDocument xDoc)
        {
            PhysicalData = physicalData;
            XDocument = xDoc;
        }

        #region Statics
        public static IDevice ReadLocation(IPhysicalData physicalData)
        {
            IDevice device = null;
            if (Uri.IsWellFormedUriString(physicalData.XmlDescriptionPath, UriKind.RelativeOrAbsolute))
            {
                HttpWebRequest request = WebRequest.Create(physicalData.XmlDescriptionPath) as HttpWebRequest;
                Task<WebResponse> async_response = request.GetResponseAsync();
                async_response.Wait();
                WebResponse response = async_response.Result;
                var stream = response.GetResponseStream();
                int length = (int)response.ContentLength;
                byte[] buffer = new byte[length];
                int received = 0;
                while (received < length)
                {
                    received += stream.Read(buffer, received, length - received);
                }

                XDocument doc = XDocument.Parse(Encoding.UTF8.GetString(buffer, 0, length));
                if (isBoseSoundTouchDevice(doc, DeviceType.MediaRenderer))
                {
                    device = new BoseSoundTouchDevice(physicalData, doc);
                }
                else
                {
                    device = new Device(physicalData, doc);
                }
            }

            return device;
        }

        private static bool isBoseSoundTouchDevice(XDocument doc, DeviceType type)
        {
            bool result = type == DeviceType.MediaRenderer;
            if (result && (doc != null))
            {
                var boseDevices = doc.DescendantNodes()
                        .Where(n => (n.NodeType == XmlNodeType.Element))
                        .Select(n => n as XElement)
                            .Where(n => n.Name.LocalName.ToString() == "manufacturer")
                            .Select(n => n)
                                .Where(n => n.Value.ToUpper().Contains("BOSE"))
                                .Select(n => n);
                result = boseDevices.Count() > 0;
            }

            return result;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion Statics

        #region Properties
        public IPhysicalData PhysicalData { get; private set; }

        protected XDocument XDocument { get; private set; }

        public bool IsServer
        {
            get
            {
                return PhysicalData.Type == DeviceType.MediaServer;
            }
        }

        public string DeviceName
        {
            get
            {
                string result = string.Empty;
                if (XDocument != null)
                {
                    if (XDocument != null)
                    {
                        var boseDevices = XDocument.DescendantNodes()
                                .Where(n => (n.NodeType == XmlNodeType.Element))
                                .Select(n => n as XElement)
                                    .Where(n => n.Name.LocalName.ToString() == "friendlyName")
                                    .Select(n => n);
                        result = boseDevices.ElementAt(0).Value;
                    }
                }
                return result;
            }
        }

        public string DeviceTypeName
        {
            get
            {
                string result = string.Empty;
                if (XDocument != null)
                {
                    if (XDocument != null)
                    {
                        var boseDevices = XDocument.DescendantNodes()
                                .Where(n => (n.NodeType == XmlNodeType.Element))
                                .Select(n => n as XElement)
                                    .Where(n => n.Name.LocalName.ToString() == "modelName")
                                    .Select(n => n);
                        result = boseDevices.ElementAt(0).Value;
                    }
                }
                return result;
            }
        }

        public string UDN
        {
            get
            {
                string result = string.Empty;
                if (XDocument != null)
                {
                    if (XDocument != null)
                    {
                        var boseDevices = XDocument.DescendantNodes()
                                .Where(n => (n.NodeType == XmlNodeType.Element))
                                .Select(n => n as XElement)
                                    .Where(n => n.Name.LocalName.ToString() == "UDN")
                                    .Select(n => n);
                        result = boseDevices.ElementAt(0).Value;
                    }
                }
                return result;
            }
        }
        #endregion Properties
    }
}
