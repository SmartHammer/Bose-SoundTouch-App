using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BoseSoundTouchApp.Models
{
    public class Response
    {
        public Response(byte[] buffer, int received)
        {
            var text = Encoding.UTF8.GetString(buffer, 0, received);
            Successful = text.StartsWith("HTTP/1.1 200 OK");
            if (Successful)
            {
                DeviceType = text.Contains("urn:schemas-upnp-org:device:" + DeviceType.MediaRenderer + ":1") ?
                               DeviceType.MediaRenderer :
                                    text.Contains("urn:schemas-upnp-org:device:" + DeviceType.MediaServer.ToString() + ":1") ?
                                    DeviceType.MediaServer :
                                    DeviceType.Other;
                if ( new List<DeviceType> { DeviceType.MediaRenderer, DeviceType.MediaServer }.Contains( DeviceType ) )
                {
                    var lines = text.Split('\n', '\r');
                    var locations = from line in lines
                                    where line.ToUpper().Contains("LOCATION") && line.ToUpper().Contains(".XML")
                                    select line;
                    if (locations.Count() > 0)
                    {
                        var location = locations.ElementAt(0);
                        var match = Regex.Match(location, @"\b(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\b");
                        var ipString = match.Captures[0].Value;
                        IPAddress = IPAddress.Parse(ipString);
                        XMLDescription = location.Substring(location.IndexOf("http:"));
                    }
                }
            }
        }

        public bool Successful
        {
            get;
            private set;
        }

        public DeviceType DeviceType
        {
            get;
            private set;
        }

        public IPAddress IPAddress
        {
            get;
            private set;
        }

        public string XMLDescription
        {
            get;
            private set;
        }

        public bool XMLDescriptionValid
        {
            get
            {
                return null != XMLDescription;
            }
        }
    }
}
