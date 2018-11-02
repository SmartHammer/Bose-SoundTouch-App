using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace BoseSoundTouchApp.Models
{
    public interface IDevice: IDisposable
    {
        IPhysicalData PhysicalData
        {
            get;
        }

        bool IsServer
        {
            get;
        }

        string DeviceName
        {
            get;
        }

        string DeviceTypeName
        {
            get;
        }

        string UDN
        {
            get;
        }
    }
}
