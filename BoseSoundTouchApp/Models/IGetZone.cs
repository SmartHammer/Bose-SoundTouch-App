using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoseSoundTouchApp.Models
{
    public interface IMember
    {
        // Element. The deviceID unique identifier of the product.
        string productID
        { get; }

        // Attribute. The IP address of the product.
        string ipaddress
        { get; }
    }

    public interface IGetZone
    {
        // Attribute. The deviceID unique identifier of the master product.
        string master
        { get; }

        // Attribute. UNSUPPORTED
        string senderIPAddress
        { get; }

        // Attribute. UNSUPPORTED
        bool senderIsMaster
        { get; }

        List<IMember> member
        { get; }
    }
}
