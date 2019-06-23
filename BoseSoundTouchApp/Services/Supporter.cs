using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace BoseSoundTouchApp.Services
{
    public class Supporter
    {
        public static XDocument Get(IPAddress ipAddress, string serviceName)
        {
            string url = "http://" + ipAddress.ToString() + ":8090";
            string requestString = url + "/" + serviceName;
            XDocument doc = null;
            using (var client = new WebClient())
            {
                var responseString = client.DownloadString(requestString);
                try
                {
                    doc = XDocument.Parse(responseString);
                }
                catch(Exception)
                {}
            }

            return doc;
        }

        public static Image GetImage(string url)
        {
            Image image = new Image();
            using (var client = new WebClient())
            {
                var data = client.DownloadData(url);
                BitmapImage dib;
                using (var stream = new MemoryStream(data))
                {
                    dib = new BitmapImage();
                    dib.SetSource(stream.AsRandomAccessStream());
                    image.Source = dib;
                }
            }

            return image;
        }

        public static T GetValue<T>(XContainer doc, [CallerMemberName] string key = "")
        {
            T result = default(T);
            if (doc != null)
            {
                var value = doc.DescendantNodes()
                    .Where(n => (n.NodeType == XmlNodeType.Element))
                    .Select(n => n as XElement)
                        .Where(n => n.Name.LocalName.ToString() == key)
                        .Select(n => n.Value);
                try
                {
                    if (value.Count() > 0)
                    {
                        var converter = TypeDescriptor.GetConverter(typeof(T));
                        if (converter != null)
                        {
                            result = (T)converter.ConvertFromString(value.ElementAt(0));
                        }
                    }
                }
                catch (NotSupportedException)
                {
                }

            }

            return result;
        }

        public static T GetAttribute<T>(XContainer doc, string item, [CallerMemberName] string attribute = "")
        {
            T result = default(T);
            if (doc != null)
            {
                var value = doc.DescendantNodes()
                    .Where(n => (n.NodeType == XmlNodeType.Element))
                    .Select(n => n as XElement)
                        .Where(n => n.Name.LocalName.ToString() == item)
                        .Select(n => n)
                            .Where(n => n.Attributes(attribute).Count() > 0)
                            .Select(n => n.Attribute(attribute).Value);
                try
                {
                    if (value.Count() > 0)
                    {
                        var converter = TypeDescriptor.GetConverter(typeof(T));
                        if (converter != null)
                        {
                            result = (T)converter.ConvertFromString(value.ElementAt(0));
                        }
                    }
                }
                catch (NotSupportedException)
                {
                }
            }

            return result;
        }

        public static T GetLocalAttribute<T>(XContainer doc, string attribute)
        {
            T result = default(T);
            if (doc != null)
            {
                if (doc is XElement)
                {
                    var xdoc = doc as XElement;
                    var atts = xdoc.Attributes(attribute);
                    if (atts.Count() > 0)
                    {
                        try
                        {
                            var converter = TypeDescriptor.GetConverter(typeof(T));
                            if (converter != null)
                            {
                                result = (T)converter.ConvertFromString(atts.ElementAt(0).Value);
                            }
                        }
                        catch (NotSupportedException)
                        {
                        }
                    }
                }
            }

            return result;
        }
    }
}
