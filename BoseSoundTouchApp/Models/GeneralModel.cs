using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BoseSoundTouchApp.Bases;
using Windows.Storage;

namespace BoseSoundTouchApp.Models
{
    public class GeneralModel : NotiferClass, IGeneralModel
    {
        public GeneralModel(IDeviceSearcher searcher, IScreen screen)
        {
            DeviceSearcher = searcher;
            Screen = screen;
            searcher.SearchFinished += new DeviceSearcherFinished.Handler((o, e) =>
            {
                if (e.Success)
                {
                    foreach (var device in Devices)
                    {
                        (device as Device).PropertyChanged += Notifier;
                        (device as BoseSoundTouchDevice)?.Initialize();
                    }
                }

                bool success = Devices.Count(dev => dev is IBoseSoundTouchDevice) > 0;
                ModelInitializationFinished(this, new DeviceSearcherEventArgs(success));
            });
            (searcher as DeviceSearcher).PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler((o, e) =>
            {
                if (e.PropertyName == "Devices")
                {
                    OnPropertyChanged("Devices");
                }
            });
        }

        public event ModelInitialized.Handler ModelInitializationFinished;

        public List<IDevice> Devices
        {
            get
            {
                return DeviceSearcher.Devices;
            }
        }

        public IScreen Screen
        {
            get;
            private set;
        }

        public bool StandBy
        {
            get;
        }

        public IPreset Presets
        {
            get;
        }

        public IVolume Volume
        {
            get;
        }

        public ISourceInfo TrackInfo
        {
            get;
        }

        public ISourceInfo SourceInfo
        {
            get;
        }

        public void Start()
        {
            DeviceSearcher.Start();
        }

        public void Stop()
        {
            DeviceSearcher.Stop();
        }

        private IDeviceSearcher DeviceSearcher
        {
            get;
            set;
        }

        private void Notifier(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }
    }
}
