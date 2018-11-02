using BoseSoundTouchApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BoseSoundTouchApp.Services
{
    public class POST
    {
        #region Members
        IPAddress m_ipAddress;
        #endregion Members

        public POST(IPhysicalData physicalData)
        {
            m_ipAddress = physicalData.Address;
        }

        public string Post(Dictionary<string, string> data)
        {
            var serviceName = GetType().Name.Substring("POST_".Length);
            string url = "http://" + m_ipAddress.ToString() + ":8090" + "/" + serviceName;
            string xml = "";
            foreach (var item in data)
            {
                xml += "<" + item.Key + ">" + item.Value + "</" + item.Key + ">";
            }

            return postXmlDataAsync(url, xml);
        }

        private string postXmlDataAsync(string url, string data)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(data);
            request.ContentType = "text/xml; encoding=utf-8";
            request.ContentLength = bytes.Length;
            request.Method = "POST";
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }
            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                string responseStr = new StreamReader(responseStream).ReadToEnd();
                return responseStr;
            }

            return string.Empty;
        }
    }
}
