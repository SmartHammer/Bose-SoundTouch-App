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
        private readonly DispatcherTimer m_dispatcherTimer;
        private readonly DispatcherTimer m_clockTimer;
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
            m_clockTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1.0)
            };
            m_clockTimer.Tick += new EventHandler<object>((o, e) =>
            {
                DateTime = DateTime.Now;
            }
            );
            m_clockTimer.Start();
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
            private get
            {
                return m_dispatcher;
            }

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
        private string m_deviceName = default(string);

        [Device(CURRENT)]
        public string DeviceName
        {
            get
            {
                return m_deviceName;
            }

            private set
            {
                var deviceName = (CurrentDevice != null) ? CurrentDevice.DeviceName : string.Empty;
                if (deviceName != m_deviceName)
                {
                    m_deviceName = deviceName;
                    OnPropertyChanged();
                }
            }
        }

        private Uri m_dataSource = default(Uri);

        [Device(CURRENT)]
        public Uri DataSource
        {
            get
            {
                return m_dataSource;
            }

            private set
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
                Uri dataSource = null;
                if (Uri.IsWellFormedUriString(result, UriKind.RelativeOrAbsolute))
                {
                    dataSource = new Uri(result);
                }

                if (dataSource != m_dataSource)
                {
                    m_dataSource = dataSource;
                    OnPropertyChanged();
                }
            }
        }

        private Uri m_backgroundImage = default(Uri);

        [Device(CURRENT)]
        public Uri BackgroundImage
        {
            get
            {
                return m_backgroundImage;
            }

            private set
            {
                var source = (CurrentDevice as Models.IBoseSoundTouchDevice).TrackInfo.art;
                if (string.IsNullOrEmpty(source) || !Uri.IsWellFormedUriString(source, UriKind.RelativeOrAbsolute))
                {
                    source = "ms-appx://" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "/Assets/DefaultBackground.jpg";
                }

                var backgroundImage = new Uri(source);
                if (backgroundImage != m_backgroundImage)
                {
                    m_backgroundImage = backgroundImage;
                    OnPropertyChanged();
                }
            }
        }

        private Uri m_speakerIcon = default(Uri);

        [Device(CURRENT)]
        public Uri SpeakerIcon
        {
            get
            {
                return m_speakerIcon;
            }

            private set
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

                Uri speakerIon = null;
                if (Uri.IsWellFormedUriString(source, UriKind.RelativeOrAbsolute))
                {
                    speakerIon = new Uri(source);
                }

                if (speakerIon != m_speakerIcon)
                {
                    m_speakerIcon = speakerIon;
                    OnPropertyChanged();
                }
            }
        }

        private double m_volume = default(double);
        private void SetVolume(double value)
        {
            var volume = (CurrentDevice as Models.IBoseSoundTouchDevice).Volume;
            (CurrentDevice as Models.IBoseSoundTouchDevice).Volume = new Volume(volume.deviceID, (int)value, volume.actualvolume, volume.muteenabled);
        }

        [Device(CURRENT)]
        public double Volume
        {
            get
            {
                return m_volume;
            }
            set
            {
                var volume = (CurrentDevice as Models.IBoseSoundTouchDevice).Volume.targetvolume;
                if (volume != m_volume)
                {
                    m_volume = volume;
                    OnPropertyChanged();
                }
            }
        }

        private bool? m_tuneIn = default(bool?);

        [Device(CURRENT)]
        public bool TuneIn
        {
            get
            {
                if (m_tuneIn != null)
                {
                    return m_tuneIn.Value;
                }
                else
                {
                    return false;
                }
            }

            private set
            {
                var tuneIn = (CurrentDevice as Models.IBoseSoundTouchDevice).SourceInfo.source == "TUNEIN";
                if (tuneIn != m_tuneIn)
                {
                    m_tuneIn = tuneIn;
                    OnPropertyChanged();
                    OnPropertyChanged("OtherSource");
                }
            }
        }

        [Device(CURRENT)]
        public bool OtherSource
        {
            get
            {
                if (m_tuneIn != null)
                {
                    return ! m_tuneIn.Value;
                }
                else
                {
                    return false;
                }
            }
        }


        private Uri m_presetIcon = default(Uri);

        [Device(CURRENT)]
        public Uri PresetIcon
        {
            get
            {
                return m_presetIcon;
            }

            private set
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

                Uri presetIcon = null;
                if (Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute))
                {
                    presetIcon = new Uri(path);
                }

                if (presetIcon != m_presetIcon)
                {
                    m_presetIcon = presetIcon;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime m_dateTime = DateTime.Now;
        public DateTime DateTime
        {
            get
            {
                return m_dateTime;
            }
            set
            {
                if ((value != m_dateTime) && ((value.ToString("HH.mm") != m_dateTime.ToString("HH.mm"))))
                {
                    m_dateTime = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion Running Page
        #endregion Properties
        #region Preset Page
        #endregion Preset Page

        #region Device Selection Page
        private bool m_prevousOpacity = default(bool);

        [Device(CURRENT)]
        public bool PreviousOpacity
        {
            get
            {
                return m_prevousOpacity;
            }

            private set
            {
                var devices = (from dev in m_model.Devices
                               where dev is BoseSoundTouchDevice
                               select dev).OrderBy(dev => dev.UDN).ToList();
                var current = CurrentDevice;
                var previousOpacity = devices.IndexOf(current) != 0;
                if (previousOpacity != m_prevousOpacity)
                {
                    m_prevousOpacity = previousOpacity;
                    OnPropertyChanged();
                }
            }
        }

        private bool m_nextOpacity = default(bool);

        [Device(CURRENT)]
        public bool NextOpacity
        {
            get
            {
                return m_nextOpacity;
            }

            private set
            {
                var devices = (from dev in m_model.Devices
                               where dev is BoseSoundTouchDevice
                               select dev).OrderBy(dev => dev.UDN).ToList();
                var current = CurrentDevice;
                var nextOpacity = devices.IndexOf(current) != (devices.Count() - 1);
                if (nextOpacity != m_nextOpacity)
                {
                    m_nextOpacity = nextOpacity;
                    OnPropertyChanged();
                }
            }
        }

        private bool? m_deviceImageOpacity = default(bool?);

        [Device(CURRENT)]
        public bool DeviceImageOpacity
        {
            get
            {
                if (m_deviceImageOpacity != null)
                {
                    return m_deviceImageOpacity.Value;
                }
                else
                {
                    return false;
                }
            }

            private set
            {
                var deviceImageOpacity = !(CurrentDevice as Models.IBoseSoundTouchDevice).State.standBy;
                if (deviceImageOpacity != m_deviceImageOpacity)
                {
                    if (!deviceImageOpacity)
                    {
                        m_dispatcherTimer.Interval = StandbyTimeout;
                        m_dispatcherTimer.Start();
                    }
                    else
                    {
                        m_dispatcherTimer.Stop();
                        m_model.Screen.Dimming(false);
                    }

                    m_deviceImageOpacity = deviceImageOpacity;
                    OnPropertyChanged("DeviceImageOpacity");
                }
            }
        }

        private Uri m_deviceTypeImagePath = default(Uri);

        public void VolumeDown()
        {
            SetVolume(Math.Max(0.0, Volume - 1.0));
        }

        public void VolumeUp()
        {
            SetVolume(Math.Min(100.0, Volume + 1.0));
        }

        [Device(ALL)]
        public Uri DeviceTypeImagePath
        {
            get
            {
                return m_deviceTypeImagePath;
            }

            private set
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
                Uri deviceTypeImagePath = null;
                if (Uri.IsWellFormedUriString(result, UriKind.RelativeOrAbsolute))
                {
                    deviceTypeImagePath = new Uri(result);
                }

                if (deviceTypeImagePath != this.m_deviceTypeImagePath)
                {
                    this.m_deviceTypeImagePath = deviceTypeImagePath;
                    OnPropertyChanged("DeviceTypeImagePath");
                }
            }
        }

        public void PreviousDevice()
        {
            SetNewDevice(-1);
        }

        public void NextDevice()
        {
            SetNewDevice(+1);
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

        public void OnOff()
        {
            var state = (CurrentDevice as Models.IBoseSoundTouchDevice).State.standBy;
            (CurrentDevice as Models.IBoseSoundTouchDevice).State = new Models.DeviceState(!state);
        }

        private void SetNewDevice(int offset)
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

        private async void Notifier(object sender, PropertyChangedEventArgs e)
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
                        var setters = from accessor in prop.GetAccessors(true)
                                      where accessor.ReturnType == typeof(void)
                                      select accessor;
                        if (setters.Count() == 1)
                        {
                            MethodInfo setter = setters.ElementAt(0);
                            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => setter.Invoke(this, new object[] { null }));
                        }
                    }
                }
            }

            TuneInViewModel.Notifier(sender, e);
            OthersViewModel.Notifier(sender, e);
            PresetSelectionViewModel.Notifier(sender, e);
            DeviceSelectionViewModel.Notifier(sender, e);
        }
    }
}
