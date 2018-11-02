using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BoseSoundTouchApp.Models
{
    public class Request
    {
        #region Members
        string m_method;
        string m_uniformResourceIdentifier;
        string m_httpProtocolVersion;
        IPEndPoint m_host;
        string m_mandatoryExtension;
        string m_searchTarget;
        int m_maximumWait;
        #endregion Members

        public Request( string method, 
                        string uniformResourceIdentifier, 
                        string httpProtocolVersion,
                        IPEndPoint host,
                        string mandatoryExtension,
                        string searchTarget,
                        int maximumWait)
        {
            m_method = method;
            m_uniformResourceIdentifier = uniformResourceIdentifier;
            m_httpProtocolVersion = httpProtocolVersion;
            m_host = host;
            m_mandatoryExtension = mandatoryExtension;
            m_searchTarget = searchTarget;
            m_maximumWait = maximumWait;
        }

        public override string ToString()
        {
            string toString = m_method + " " + m_uniformResourceIdentifier + " " + m_httpProtocolVersion + "\r\n";
            toString += "HOST" + ":" + m_host + "\r\n";
            toString += "MAN" + ":" + "\"" + m_mandatoryExtension + "\"" + "\r\n";
            toString += "ST" + ":" + m_searchTarget + "\r\n";
            toString += "MX" + ":" + m_maximumWait + "\r\n";
            toString += "\r\n";
            return toString;
        }

        public static implicit operator string(Request command)
        {
            return command.ToString();
        }

        public static implicit operator byte[](Request command)
        {
            return Encoding.UTF8.GetBytes(command);
        }
    }
}
