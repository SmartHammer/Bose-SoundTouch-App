using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Windows.Storage;
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
            //while (null == doc)
            {
                HttpWebRequest request = HttpWebRequest.Create(requestString) as HttpWebRequest;
                Task<WebResponse> async_response = request.GetResponseAsync();
                async_response.Wait();
                WebResponse response = async_response.Result;
                var stream = response.GetResponseStream();
                byte[] buffer = new byte[16384];
                int received = stream.Read(buffer, 0, buffer.Length);
                string text = Encoding.UTF8.GetString(buffer, 0, received);
                try
                {
                    doc = XDocument.Parse(text);
                }
                catch (Exception al)
                {
                    doc = null;
                }
            }

            return doc;
        }

        public static Image GetImage(string url)
        {
            Image image = new Image();
            var httpClient = new HttpClient();
            var request = httpClient.GetStreamAsync(url);
            request.Wait();
            Stream st = request.Result;
            var memoryStream = new MemoryStream();
            var copyRequest = st.CopyToAsync(memoryStream);
            copyRequest.Wait();
            memoryStream.Position = 0;
            BitmapImage bitmap = new BitmapImage();
            bitmap.SetSource(memoryStream.AsRandomAccessStream());
            image.Source = bitmap;
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
