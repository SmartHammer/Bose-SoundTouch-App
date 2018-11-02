using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoseSoundTouchApp.Models
{
    public enum RegistrationTarget
    {
        Current,
        All
    }

    public class ModelInitializedEventArgs : EventArgs
    {
        public ModelInitializedEventArgs(bool success)
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

    public class ModelInitialized
    {
        public delegate void Handler(object sender, DeviceSearcherEventArgs args);
    }

    public interface IGeneralModel
    {
        event ModelInitialized.Handler ModelInitializationFinished;

        List<IDevice> Devices
        {
            get;
        }

        IScreen Screen
        {
            get;
        }

        void Start();

        void Stop();
    }
}
