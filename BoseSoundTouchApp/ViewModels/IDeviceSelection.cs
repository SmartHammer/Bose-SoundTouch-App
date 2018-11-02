using System.Collections.ObjectModel;

namespace BoseSoundTouchApp.ViewModels
{
    public interface IDeviceSelection
    {
        ObservableCollection<IBoseSoundTouchDevice> Devices
        {
            get;
        }
    }
}
