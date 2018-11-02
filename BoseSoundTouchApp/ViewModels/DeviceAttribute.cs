using System;

namespace BoseSoundTouchApp.ViewModels
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=true)
]
    public class DeviceAttribute : Attribute
    {
        public DeviceAttribute(string device)
        {
            Device = device;
        }

        public string Device
        {
            get;
            private set;
        }
    }
}
