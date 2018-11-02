using System;
using System.Collections.Generic;

namespace BoseSoundTouchApp.Models
{
    public class DeviceSearcherEventArgs : EventArgs
    {
        public DeviceSearcherEventArgs(bool success)
            : base()
        {
            Success = success;
        }

        public bool Success
        {
            get;
            private set;
        }
    }

    public class DeviceSearcherFinished
    {
        public delegate void Handler(object sender, DeviceSearcherEventArgs args);
    }

    public interface IDeviceSearcher
    {
        event DeviceSearcherFinished.Handler SearchFinished;

        List<IDevice> Devices
        {
            get;
        }

        void Start();
        void Stop();
    }
}