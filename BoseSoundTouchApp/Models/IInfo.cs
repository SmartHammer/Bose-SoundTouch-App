using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoseSoundTouchApp.Models
{
    public interface IComponent
    {
        string componentCategory
        {
            get;
        }

        string softwareVersion
        {
            get;
        }

        string serialNumber
        {
            get;
        }
    }

    public interface INetworkInfo
    {
        string type
        {
            get;
        }

        string macAddress
        {
            get;
        }

        string ipAddress
        {
            get;
        }
    }

    public interface IInfo
    {
        string deviceID
        {
            get;
        }

        string name
        {
            get;
        }

        string type
        {
            get;
        }

        IComponent component
        {
            get;
        }

        string margeAccountUUID
        {
            get;
        }

        INetworkInfo networkInfo
        {
            get;
        }

        string moduleType
        {
            get;
        }

        string variant
        {
            get;
        }

        string variantMode
        {
            get;
        }

        string countryCode
        {
            get;
        }

        string regionCode
        {
            get;
        }
    }
}
