using BoseSoundTouchApp.Bases;
using BoseSoundTouchApp.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Windows.Storage;

namespace BoseSoundTouchApp.ViewModels
{
    public class TuneInViewModel : NotiferClass
    {
        #region Members
        private const string ALL = "All";
        private const string CURRENT = "CurrentDevice";
        #endregion Members

        public Models.IGeneralModel Model
        {
            private get;
            set;
        }

        public Windows.UI.Core.CoreDispatcher Dispatcher
        {
            private get;
            set;
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

        private string track = default(string);
        private void SetTrack()
        {
            var value = (CurrentDevice as Models.IBoseSoundTouchDevice).TrackInfo.artist;
            if (!string.IsNullOrEmpty(value))
            {
                var parts = value.Split(" - ");
                if (parts.Count() > 1)
                {
                    value = parts[1];
                    if ((value.Length > 15) && value.Contains(" (") )
                    {
                        value = value.Replace(" (", "\n(");
                    }
                }
            }
            else
            {
                value = string.Empty;
            }

            if (value != track)
            {
                track = value;
                OnPropertyChanged("Track");
            }
        }

        [Device(CURRENT)]
        public string Track
        {
            get
            {
                return track;
            }
        }

        private string artist = default(string);
        private void SetArtist()
        {
            var value = (CurrentDevice as Models.IBoseSoundTouchDevice).TrackInfo.artist;
            if (!string.IsNullOrEmpty(value))
            {
                var parts = value.Split(" - ");
                if (parts.Count() > 1)
                {
                    value = parts[0];
                }
            }
            else
            {
                value = string.Empty;
            }

            if (value != artist)
            {
                artist = value;
                OnPropertyChanged("Artist");
            }
        }

        [Device(CURRENT)]
        public string Artist
        {
            get
            {
                return artist;
            }
        }

        private Uri art = default(Uri);
        private void SetArt()
        {
            Uri value = null;
            switch ((CurrentDevice as Models.IBoseSoundTouchDevice).TrackInfo.art_status)
            {
                case ART_STATUS.IMAGE_PRESENT:
                    var _ = (CurrentDevice as Models.IBoseSoundTouchDevice).TrackInfo.art;
                    if (Uri.IsWellFormedUriString(_, UriKind.RelativeOrAbsolute))
                    {
                        value = new Uri(_);
                    }
                    break;
                case ART_STATUS.SHOW_DEFAULT_IMAGE:
                    break;
                case ART_STATUS.DOWNLOADING:
                    break;
                case ART_STATUS.INVALID:
                default:
                    break;
            }

            if (value != art)
            {
                art = value;
                OnPropertyChanged("Art");
            }
        }

        [Device(CURRENT)]
        public Uri Art
        {
            get
            {
                return art;
            }
        }

        private Uri stationIcon = default(Uri);
        private void SetStationIcon()
        {
            Uri value = null;
            var art = (CurrentDevice as Models.IBoseSoundTouchDevice).SourceInfo.sourceArt;
            if (Uri.IsWellFormedUriString(art, UriKind.RelativeOrAbsolute))
            {
                value = new Uri(art);
            }

            if (value != stationIcon)
            {
                stationIcon = value;
                OnPropertyChanged("StationIcon");
            }
        }

        [Device(CURRENT)]
        public Uri StationIcon
        {
            get
            {
                return stationIcon;
            }
        }

        private string stationName = default(string);
        private void SetStationName()
        {
            var value = (CurrentDevice as Models.IBoseSoundTouchDevice).SourceInfo.sourceName;
            if(value != stationName)
            {
                stationName = value;
                OnPropertyChanged("StationName");
            }
        }

        [Device(CURRENT)]
        public string StationName
        {
            get
            {
                return stationName;
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

        public void Notifier(object sender, PropertyChangedEventArgs e)
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
                        var setter = from method in GetType().GetRuntimeMethods()
                                     where method.Name == "Set" + prop.Name
                                     select method;
                        if (setter.Count() > 0)
                        {
                            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                            {
                                setter.ElementAt(0).Invoke(this, new object[] { });
                            });
                        }
                    }
                }
            }
        }

        public void OnTick()
        {
            var properties = this.GetType().GetProperties(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.GetProperty);
            foreach (var property in properties)
            {
                OnPropertyChanged(property.Name);
            }
        }
    }
}
