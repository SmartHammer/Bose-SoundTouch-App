using System.Net;

namespace BoseSoundTouchApp.Models
{
    public enum DeviceType
    {
        MediaServer,
        MediaRenderer,
        Other
    };

    public interface IPhysicalData
    {
        /// <summary>
        /// Gets the IPAddress of the device
        /// </summary>
        IPAddress Address
        {
            get;
        }

        /// <summary>
        /// Gets the DeviceType of the device
        /// </summary>
        DeviceType Type
        {
            get;
        }

        /// <summary>
        /// Gets the XML description path of the device
        /// </summary>
        string XmlDescriptionPath
        {
            get;
        }
    }
}
