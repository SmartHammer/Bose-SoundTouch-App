using BoseSoundTouchApp.Bases;
using BoseSoundTouchApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Windows.Storage;
using Windows.UI.Xaml.Input;

namespace BoseSoundTouchApp.ViewModels
{
    [DataContract]
    public class PresetSelectionViewModel : NotiferClass
    {
        #region Members
        private const string ALL = "All";
        private const string CURRENT = "CurrentDevice";
        #endregion Members

        public IGeneralModel Model
        {
            private get;
            set;
        }

        public Windows.UI.Core.CoreDispatcher Dispatcher
        {
            private get;
            set;
        }

        private ObservableCollection<Preset> m_presets = default(ObservableCollection<Preset>);

        private Preset GetPreset(int index)
        {
            var presets = (CurrentDevice as Models.IBoseSoundTouchDevice).Presets.Presets;
            Preset preset = new Preset
            {
                Number = (uint)index
            };

            if (presets.ContainsKey(index))
            {
                var p = presets[index];
                preset.Icon = new Uri(p.ContentItemContainerArt);
                preset.Name = p.ContentItemName;
                string result = default(string);
                string prefix = "ms-appx://" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "/Assets/";
                switch (p.Source)
                {
                    case "INTERNET_RADIO":
                        result = "default_source.png";
                        preset.TypeName = "Internet Radio";
                        break;
                    case "PRODUCT":
                        result = "TV.png";
                        preset.TypeName = "Product";
                        break;
                    case "BLUETOOTH":
                        result = "bluetooth.png";
                        preset.TypeName = "Bluetooth";
                        break;
                    case "AUX":
                        result = "aux_input.png";
                        preset.TypeName = "Aux";
                        break;
                    case "TUNEIN":
                        result = "TuneIn.png";
                        preset.TypeName = "TuneIn";
                        break;
                    case "STORED_MUSIC":
                        result = "Cloud.png";
                        preset.TypeName = "Stored Music";
                        break;
                    default:
                        result = "";
                        preset.TypeName = "unknown";
                        break;
                }

                result = prefix + result;
                if (Uri.IsWellFormedUriString(result, UriKind.RelativeOrAbsolute))
                {
                    preset.Type = new Uri(result);
                }
            }

            return preset;
        }

        public void Select(int index)
        {
            var presets = (CurrentDevice as Models.IBoseSoundTouchDevice).Presets.Presets;
            var sourceInfo = (CurrentDevice as Models.IBoseSoundTouchDevice).SourceInfo;
            var path = "ms-appx://" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "/Assets/";

            var preset = from p in presets
                         where p.Value.ContentItemName == sourceInfo.sourceName
                         select p;
            if (preset.Count() > 0)
            {
                var selected = preset.ElementAt(0).Key;
                if (selected != index)
                {
                    if (presets.ContainsKey(index))
                    {
                        (CurrentDevice as Models.IBoseSoundTouchDevice).SelectPreset(index);
                    }
                }
            }
        }

        public Preset Preset1
        {
            get
            {
                return GetPreset(1);
            }
        }

        public Preset Preset2
        {
            get
            {
                return GetPreset(2);
            }
        }

        public Preset Preset3
        {
            get
            {
                return GetPreset(3);
            }
        }

        public Preset Preset4
        {
            get
            {
                return GetPreset(4);
            }
        }

        public Preset Preset5
        {
            get
            {
                return GetPreset(5);
            }
        }

        public Preset Preset6
        {
            get
            {
                return GetPreset(6);
            }
        }

        [Device(CURRENT)]
        [DataMember]
        public ObservableCollection<Preset> Presets
        {
            get
            {
                return m_presets;
            }

            private set
            {
                var presets = (CurrentDevice as Models.IBoseSoundTouchDevice).Presets.Presets;
                ObservableCollection<Preset> ps = new ObservableCollection<Preset>();
                for(var i = 0u; i < 6u; ++i)
                {
                    var preset = new Preset
                    {
                        Number = i + 1u
                    };
                    ps.Add(preset);
                }

                foreach (var preset in presets.OrderBy(p => p.Key))
                {
                    var index = preset.Key;
                    var item = ps.ElementAt(index - 1);
                    item.Icon = new Uri(preset.Value.ContentItemContainerArt);
                    item.Name = preset.Value.ContentItemName;
                    string result = default(string);
                    string prefix = "ms-appx://" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "/Assets/";
                    switch (preset.Value.Source)
                    {
                        case "INTERNET_RADIO":
                            result = "default_source.png";
                            item.TypeName = "Internet Radio";
                            break;
                        case "PRODUCT":
                            result = "TV.png";
                            item.TypeName = "Product";
                            break;
                        case "BLUETOOTH":
                            result = "bluetooth.png";
                            item.TypeName = "Bluetooth";
                            break;
                        case "AUX":
                            result = "aux_input.png";
                            item.TypeName = "Aux";
                            break;
                        case "TUNEIN":
                            result = "TuneIn.png";
                            item.TypeName = "TuneIn";
                            break;
                        case "STORED_MUSIC":
                            result = "Cloud.png";
                            item.TypeName = "Stored Music";
                            break;
                        default:
                            result = "";
                            item.TypeName = "unknown";
                            break;
                    }

                    result = prefix + result;
                    if (Uri.IsWellFormedUriString(result, UriKind.RelativeOrAbsolute))
                    {
                        item.Type = new Uri(result);
                    }
                }

                if (( m_presets == null) || ! ps.SequenceEqual(second: m_presets))
                {
                    m_presets = ps;
                    OnPropertyChanged();
                    for (var i = 1; i <= 6; ++i)
                    {
                        OnPropertyChanged("Preset" + i.ToString());
                    }
                }
            }
        }

        private IDevice CurrentDevice
        {
            get
            {
                Models.Settings settings = ApplicationData.Current.LocalSettings.Values["PlayerSettings"]
                                as ApplicationDataCompositeValue;
                var current = from dev in Model.Devices
                              where dev.UDN == settings.UDN
                              select dev;
                return current.Count() > 0 ? current.ElementAt(0) : null;
            }
        }

        public async void Notifier(object sender, PropertyChangedEventArgs e)
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
            var properties = this.GetType().GetProperties(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.GetProperty |
                BindingFlags.DeclaredOnly);
            foreach (var prop in properties)
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
        }
    }
}
