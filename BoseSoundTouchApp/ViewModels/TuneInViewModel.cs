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

            set
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

        private string m_track = default(string);

        [Device(CURRENT)]
        public string Track
        {
            get
            {
                return m_track;
            }

            private set
            {
                var track = (CurrentDevice as Models.IBoseSoundTouchDevice).TrackInfo.artist;
                if (!string.IsNullOrEmpty(track))
                {
                    var parts = track.Split(" - ");
                    if (parts.Count() > 1)
                    {
                        track = parts[1];
                        if ((track.Length > 15) && track.Contains(" ("))
                        {
                            track = track.Replace(" (", "\n(");
                        }
                    }
                }
                else
                {
                    track = string.Empty;
                }

                if (track != m_track)
                {
                    m_track = track;
                    OnPropertyChanged();
                }
            }
        }

        private string m_artist = default(string);

        [Device(CURRENT)]
        public string Artist
        {
            get
            {
                return m_artist;
            }

            private set
            {
                var artist = (CurrentDevice as Models.IBoseSoundTouchDevice).TrackInfo.artist;
                if (!string.IsNullOrEmpty(artist))
                {
                    var parts = artist.Split(" - ");
                    if (parts.Count() > 1)
                    {
                        artist = parts[0];
                    }
                }
                else
                {
                    artist = string.Empty;
                }

                if (artist != m_artist)
                {
                    m_artist = artist;
                    OnPropertyChanged();
                }
            }
        }

        private Uri m_art = default(Uri);

        [Device(CURRENT)]
        public Uri Art
        {
            get
            {
                return m_art;
            }

            private set
            {
                Uri art = null;
                switch ((CurrentDevice as Models.IBoseSoundTouchDevice).TrackInfo.art_status)
                {
                    case ART_STATUS.IMAGE_PRESENT:
                        var _ = (CurrentDevice as Models.IBoseSoundTouchDevice).TrackInfo.art;
                        if (Uri.IsWellFormedUriString(_, UriKind.RelativeOrAbsolute))
                        {
                            art = new Uri(_);
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

                if (art != m_art)
                {
                    m_art = art;
                    OnPropertyChanged();
                }
            }
        }

        private Uri m_stationIcon = default(Uri);

        [Device(CURRENT)]
        public Uri StationIcon
        {
            get
            {
                return m_stationIcon;
            }

            private set
            {
                Uri stationIcon = null;
                var art = (CurrentDevice as Models.IBoseSoundTouchDevice).SourceInfo.sourceArt;
                if (Uri.IsWellFormedUriString(art, UriKind.RelativeOrAbsolute))
                {
                    stationIcon = new Uri(art);
                }

                if (stationIcon != m_stationIcon)
                {
                    m_stationIcon = stationIcon;
                    OnPropertyChanged();
                }
            }
        }

        private string m_stationName = default(string);

        [Device(CURRENT)]
        public string StationName
        {
            get
            {
                return m_stationName;
            }

            private set
            {
                var stationName = (CurrentDevice as Models.IBoseSoundTouchDevice).SourceInfo.sourceName;
                if (stationName != m_stationName)
                {
                    m_stationName = stationName;
                    OnPropertyChanged();
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
