using BoseSoundTouchApp.Bases;
using BoseSoundTouchApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Core;
using Windows.UI.Xaml.Input;

namespace BoseSoundTouchApp.ViewModels
{
    public class RunningPageViewModel : NotiferClass
    {
        #region Members
        private const string ALL = "All";
        private const string CURRENT = "CurrentDevice";
        private IGeneralModel m_model;
        private CoreDispatcher m_dispatcher;
        private DispatcherTimer m_dispatcherTimer;
        #endregion Members

        public RunningPageViewModel()
        {
            TuneInViewModel = new TuneInViewModel();
            OthersViewModel = new OthersViewModel();
            PresetSelectionViewModel = new PresetSelectionViewModel();
            DeviceSelectionViewModel = new DeviceSelectionViewModel();
            m_dispatcherTimer = new DispatcherTimer();
            m_dispatcherTimer.Tick += new EventHandler<object>((o, e) =>
            {
                m_dispatcherTimer.Stop();
                m_model.Screen.Dimming(true);
            });
        }

        public void Initialize()
        {
        }

        public void UpDimming()
        {
            m_model.Screen.Dimming(false);
        }

        private TimeSpan StandbyTimeout => TimeSpan.FromMinutes(0.5);

        public IGeneralModel Model
        {
            set
            {
                m_model = value;
                TuneInViewModel.Model = m_model;
                OthersViewModel.Model = m_model;
                PresetSelectionViewModel.Model = m_model;
                DeviceSelectionViewModel.Model = m_model;
                var storedDeviceName = ApplicationData.Current.LocalSettings.Values["PlayerSettings"]
                                        as ApplicationDataCompositeValue;
                var boseDevices = from device in m_model.Devices
                                    where device is BoseSoundTouchDevice
                                    select device;
                var storedBoseDevice = from device in boseDevices
                                        where Models.Settings.@equals(storedDeviceName, device)
                                        select device;
                var dev = storedBoseDevice.Count() > 0 ?
                                storedBoseDevice.ElementAt(0) :
                                boseDevices.Count() > 0 ?
                                boseDevices.ElementAt(0) :
                                null;
                if (!Models.Settings.equals(storedDeviceName, dev))
                {
                    var settings = new Models.Settings
                    {
                        UDN = dev.UDN
                    };
                    ApplicationDataCompositeValue compositeValue = settings;
                    ApplicationData.Current.LocalSettings.Values["PlayerSettings"] = compositeValue;
                }

                (m_model as Models.GeneralModel).PropertyChanged += new PropertyChangedEventHandler((o, e) =>
                {
                    Notifier(o, e);
                });
            }
        }

        public CoreDispatcher Dispatcher
        {
            set
            {
                m_dispatcher = value;
                TuneInViewModel.Dispatcher = value;
                OthersViewModel.Dispatcher = value;
                PresetSelectionViewModel.Dispatcher = value;
                Notifier(this, new PropertyChangedEventArgs(CURRENT));
            }
        }

        #region Properties
        public TuneInViewModel TuneInViewModel
        {
            get;
            private set;
        }

        public OthersViewModel OthersViewModel
        {
            get;
            private set;
        }

        public PresetSelectionViewModel PresetSelectionViewModel
        {
            get;
            private set;
        }

        public DeviceSelectionViewModel DeviceSelectionViewModel
        {
            get;
            private set;
        }

        #region Running Page
        private string deviceName = default(string);
        private void SetDeviceName()
        {
            var value = (CurrentDevice != null) ? CurrentDevice.DeviceName : string.Empty;
            if (value != deviceName)
            {
                deviceName = value;
                OnPropertyChanged("DeviceName");
            }
        }

        [Device(CURRENT)]
        public string DeviceName
        {
            get
            {
                return deviceName;
            }
        }

        private Uri dataSource = default(Uri);
        private void SetDataSource()
        {
            var source = (CurrentDevice as Models.IBoseSoundTouchDevice).SourceInfo.source;
            string result = string.Empty;
            string prefix = "ms-appx://" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "/Assets/";
            switch (source)
            {
                case "INTERNET_RADIO":
                    result = "default_source.png";
                    break;
                case "PRODUCT":
                    result = "TV.png";
                    break;
                case "BLUETOOTH":
                    result = "bluetooth.png";
                    break;
                case "AUX":
                    result = "aux_input.png";
                    break;
                case "TUNEIN":
                    result = "TuneIn.png";
                    break;
                case "STORED_MUSIC":
                    result = "Cloud.png";
                    break;
                default:
                    result = "";
                    break;
            }

            result = prefix + result;
            Uri value = null;
            if (Uri.IsWellFormedUriString(result, UriKind.RelativeOrAbsolute))
            {
                value = new Uri(result);
            }

            if (value != dataSource)
            {
                dataSource = value;
                OnPropertyChanged("DataSource");
            }
        }

        [Device(CURRENT)]
        public Uri DataSource
        {
            get
            {
                return dataSource;
            }
        }

        private Uri backgroundImage = default(Uri);
        private void SetBackgroundImage()
        {
            var source = (CurrentDevice as Models.IBoseSoundTouchDevice).TrackInfo.art;
            if (string.IsNullOrEmpty(source) || !Uri.IsWellFormedUriString(source, UriKind.RelativeOrAbsolute))
            {
                source = "ms-appx://" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "/Assets/DefaultBackground.jpg";
            }

            var value = new Uri(source);
            if (value != backgroundImage)
            {
                backgroundImage = value;
                OnPropertyChanged("BackgroundImage");
            }
        }

        [Device(CURRENT)]
        public Uri BackgroundImage
        {
            get
            {
                return backgroundImage;
            }
        }

        private Uri speakerIcon = default(Uri);
        private void SetSpeakerIcon()
        {
            var volume = (CurrentDevice as Models.IBoseSoundTouchDevice).Volume;
            var source = "ms-appx://" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "/Assets/";

            if (volume.muteenabled)
            {
                source += "Mute.png";
            }
            else
            {
                if ((0 <= volume.targetvolume) && (volume.targetvolume <= 10))
                {
                    source += "Volume0.png";
                }
                else if ((10 < volume.targetvolume) && (volume.targetvolume <= 40))
                {
                    source += "Volume33.png";
                }
                else if ((40 < volume.targetvolume) && (volume.targetvolume <= 80))
                {
                    source += "Volume66.png";
                }
                else
                {
                    source += "Volume100.png";
                }
            }

            Uri value = null;
            if (Uri.IsWellFormedUriString(source, UriKind.RelativeOrAbsolute))
            {
                value = new Uri(source);
            }

            if (value != speakerIcon)
            {
                speakerIcon = value;
                OnPropertyChanged("SpeakerIcon");
            }
        }

        [Device(CURRENT)]
        public Uri SpeakerIcon
        {
            get
            {
                return speakerIcon;
            }
        }

        private double volume = default(double);
        private void SetVolume()
        {

            var value = (CurrentDevice as Models.IBoseSoundTouchDevice).Volume.targetvolume;
            if (value != volume)
            {
                volume = value;
                OnPropertyChanged("Volume");
            }
        }

        [Device(CURRENT)]
        public double Volume
        {
            get
            {
                return volume;
            }
            set
            {
                var volume = (CurrentDevice as Models.IBoseSoundTouchDevice).Volume;
                (CurrentDevice as Models.IBoseSoundTouchDevice).Volume = new Volume(volume.deviceID, (int)value, volume.actualvolume, volume.muteenabled);
            }
        }

        private bool? tuneIn = default(bool?);
        private void SetTuneIn()
        {
            var value = (CurrentDevice as Models.IBoseSoundTouchDevice).SourceInfo.source == "TUNEIN";
            if (value != tuneIn)
            {
                tuneIn = value;
                OnPropertyChanged("TuneIn");
                OnPropertyChanged("OtherSource");
            }
        }

        [Device(CURRENT)]
        public bool TuneIn
        {
            get
            {
                if (tuneIn != null)
                {
                    return tuneIn.Value;
                }
                else
                {
                    return false;
                }
            }
        }

        [Device(CURRENT)]
        public bool OtherSource
        {
            get
            {
                if (tuneIn != null)
                {
                    return ! tuneIn.Value;
                }
                else
                {
                    return false;
                }
            }
        }


        private Uri presetIcon = default(Uri);
        private void SetPresetIcon()
        {
            var presets = (CurrentDevice as Models.IBoseSoundTouchDevice).Presets;
            var sourceInfo = (CurrentDevice as Models.IBoseSoundTouchDevice).SourceInfo;
            var path = "ms-appx://" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "/Assets/";

            var preset = from p in presets.Presets
            where p.Value.ContentItemName == sourceInfo.sourceName
            select p;
            if (preset.Count() > 0)
            {
                var index = preset.ElementAt(0).Key;
                path += "Preset" + index.ToString() + ".png";
            }
            else
            {
                path += "Preset.png";
            }

            Uri value = null;
            if (Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute))
            {
                value = new Uri(path);
            }

            if (value != presetIcon)
            {
                presetIcon = value;
                OnPropertyChanged("PresetIcon");
            }
        }

        [Device(CURRENT)]
        public Uri PresetIcon
        {
            get
            {
                return presetIcon;
            }
        }
        #endregion Running Page
        #endregion Properties
        #region Preset Page
        #endregion Preset Page

        #region Device Selection Page
        private bool prevousOpacity = default(bool);
        private void SetPreviousOpacity()
        {
            var devices = (from dev in m_model.Devices
                           where dev is BoseSoundTouchDevice
                           select dev).OrderBy(dev => dev.UDN).ToList();
            var current = CurrentDevice;
            var value = devices.IndexOf(current) != 0;
            if (value != prevousOpacity)
            {
                prevousOpacity = value;
                OnPropertyChanged("PreviousOpacity");
            }
        }

        [Device(CURRENT)]
        public bool PreviousOpacity
        {
            get
            {
                return prevousOpacity;
            }
        }

        private bool nextOpacity = default(bool);
        private void SetNextOpacity()
        {
            var devices = (from dev in m_model.Devices
                           where dev is BoseSoundTouchDevice
                           select dev).OrderBy(dev => dev.UDN).ToList();
            var current = CurrentDevice;
            var value = devices.IndexOf(current) != (devices.Count() - 1);
            if (value != nextOpacity)
            {
                nextOpacity = value;
                OnPropertyChanged("NextOpacity");
            }
        }

        [Device(CURRENT)]
        public bool NextOpacity
        {
            get
            {
                return nextOpacity;
            }
        }

        private bool? deviceImageOpacity = default(bool?);
        private void SetDeviceImageOpacity()
        {
            var value = !(CurrentDevice as Models.IBoseSoundTouchDevice).State.standBy;
            if (value != deviceImageOpacity)
            {
                if ( !value)
                {
                    m_dispatcherTimer.Interval = StandbyTimeout;
                    m_dispatcherTimer.Start();
                }
                else
                {
                    m_dispatcherTimer.Stop();
                    m_model.Screen.Dimming(false);
                }

                deviceImageOpacity = value;
                OnPropertyChanged("DeviceImageOpacity");
            }
        }

        [Device(CURRENT)]
        public bool DeviceImageOpacity
        {
            get
            {
                if (deviceImageOpacity != null)
                {
                    return deviceImageOpacity.Value;
                }
                else
                {
                    return false;
                }
            }
        }

        private Uri deviceTypeImagePath = default(Uri);
        private void SetDeviceTypeImagePath()
        {
            var current = CurrentDevice;
            string result = "ST10.png";
            if (current != null)
            {
                switch (current.DeviceTypeName)
                {
                    case "SoundTouch 10":
                        result = "ST10.png";
                        break;
                    case "SoundTouch 20":
                        result = "ST20.png";
                        break;
                    case "SoundTouch 30":
                        result = "ST30.png";
                        break;
                    case "SoundTouch 300":
                        result = "ST300.png";
                        break;
                    default:
                        result = "ST10.png";
                        break;
                }
            }
            string prefix = "ms-appx://" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "/Assets/";
            result = prefix + result;
            Uri value = null;
            if (Uri.IsWellFormedUriString(result, UriKind.RelativeOrAbsolute))
            {
                value = new Uri(result);
            }

            if (value != deviceTypeImagePath)
            {
                deviceTypeImagePath = value;
                OnPropertyChanged("DeviceTypeImagePath");
            }
        }

        public void VolumeDown()
        {
            var value = Volume;
            value -= 1.0;
            Volume = Math.Max(value, 0.0);
        }

        public void VolumeUp()
        {
            var value = Volume;
            value += 1.0;
            Volume = Math.Min(value, 100.0);
        }

        [Device(ALL)]
        public Uri DeviceTypeImagePath
        {
            get
            {
                return deviceTypeImagePath;
            }
            private set
            {
                OnPropertyChanged();
            }
        }

        public void PreviousDevice()
        {
            setNewDevice(-1);
        }

        public void NextDevice()
        {
            setNewDevice(+1);
        }

        public void StartRunningView()
        {
        }

        public void StartDeviceSelectionView()
        {
        }

        public void StartPresetSelectionView()
        {
        }

        private void setNewDevice(int offset)
        {
            var devices = (from dev in m_model.Devices
                           where dev is BoseSoundTouchDevice
                           select dev).OrderBy(dev => dev.UDN).ToList();
            var current = CurrentDevice;
            if (current != null)
            {
                var index = devices.IndexOf(current);
                bool next = ((index + offset) <= (devices.Count() - 1));
                bool prev = ((index + offset) >= 0);
                index = prev && next ? index + offset : index;
                Models.Settings newSettings = new Models.Settings { UDN = devices.ElementAt(index).UDN };
                ApplicationDataCompositeValue compositeValue = newSettings;
                ApplicationData.Current.LocalSettings.Values["PlayerSettings"] = compositeValue;
                WriteSettings();
                Notifier(this, new PropertyChangedEventArgs(CURRENT));
            }
        }

        async private void WriteSettings()
        {
            Models.Settings settings = ApplicationData.Current.LocalSettings.Values["PlayerSettings"] as ApplicationDataCompositeValue;
            await XmlIO.SaveObjectToXml<Settings>(settings, Settings.Path);
        }

        private IDevice CurrentDevice
        {
            get
            {
                Models.Settings settings = ApplicationData.Current.LocalSettings.Values["PlayerSettings"]
                                as ApplicationDataCompositeValue;
                var current = from dev in m_model.Devices
                              where dev.UDN == settings.UDN
                              select dev;
                return current.Count() > 0 ? current.ElementAt(0) : null;
            }
        }
        #endregion Device Selection Page

        private void Notifier(object sender, PropertyChangedEventArgs e)
        {
            string propName = e.PropertyName;
            string deviceId = string.Empty;
            var parts = e.PropertyName.Split('.');
            if (parts.Count() > 1)
            {
                propName = parts[1];
                deviceId = parts[0];
                if (propName == "StandBy")
                {
                    deviceId = string.Empty;
                }
            }
            else if (propName == CURRENT)
            {
                deviceId = CurrentDevice.UDN;
            }
            var properties = this.GetType().GetProperties(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.GetProperty |
                BindingFlags.DeclaredOnly);
            foreach(var prop in properties)
            {
                // Using reflection.  
                System.Attribute[] attrs = Attribute.GetCustomAttributes(prop);  // Reflection.  
                var depAttrs = from attr in attrs
                               where attr is DeviceAttribute
                               select attr as DeviceAttribute;
                foreach (var depAttr in depAttrs)
                {
                    if ((depAttr.Device == ALL) || 
                        ((depAttr.Device == CURRENT) && (CurrentDevice.UDN == deviceId)) ||
                        (string.Empty == deviceId))
                    {
                        var setter = from method in GetType().GetRuntimeMethods()
                                     where method.Name == "Set" + prop.Name
                                     select method;
                        if (setter.Count() > 0)
                        {
                            m_dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                            {
                                setter.ElementAt(0).Invoke(this, new object[] { });
                            });
                        }
                    }
                }
            }

            TuneInViewModel.Notifier(sender, e);
            OthersViewModel.Notifier(sender, e);
            PresetSelectionViewModel.NotifierAsync(sender, e);
            DeviceSelectionViewModel.Notifier(sender, e);
        }
    }
}
