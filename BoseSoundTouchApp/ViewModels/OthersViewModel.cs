using BoseSoundTouchApp.Bases;
using BoseSoundTouchApp.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Windows.Storage;

namespace BoseSoundTouchApp.ViewModels
{
    public class OthersViewModel : NotiferClass
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
                var tunein = (CurrentDevice as Models.IBoseSoundTouchDevice).SourceInfo.source == "TUNEIN";
                if (tunein != m_tuneIn)
                {
                    m_tuneIn = tunein;
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
                var track = (CurrentDevice as Models.IBoseSoundTouchDevice).TrackInfo.track;
                if (!string.IsNullOrEmpty(track))
                {
                    if ((track.Length > 15) && track.Contains(" ("))
                    {
                        track = track.Replace(" (", "\n(");
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

        private string m_album = default(string);

        [Device(CURRENT)]
        public string Album
        {
            get
            {
                return m_album;
            }

            private set
            {
                var album = (CurrentDevice as Models.IBoseSoundTouchDevice).TrackInfo.album;

                if (album != m_album)
                {
                    m_album = album;
                    OnPropertyChanged();
                }
            }
        }

        private string m_currentTime = default(string);

        [Device(CURRENT)]
        public string CurrentTime
        {
            get
            {
                return m_currentTime;
            }

            private set
            {
                var time = (CurrentDevice as Models.IBoseSoundTouchDevice).TrackInfo.time.ToString(@"mm\:ss");
                if (m_currentTime != time)
                {
                    m_currentTime = time;
                    OnPropertyChanged();
                }
            }
        }

        private string m_totalTime = default(string);

        [Device(CURRENT)]
        public string TotalTime
        {
            get
            {
                return m_totalTime;
            }

            private set
            {
                var time = (CurrentDevice as Models.IBoseSoundTouchDevice).TrackInfo.timeTotal.ToString(@"mm\:ss");
                if (m_totalTime != time)
                {
                    m_totalTime = time;
                    OnPropertyChanged();
                }
            }
        }

        private double? m_progress = default(double?);

        [Device(CURRENT)]
        public double Progress
        { 
            get
            {
                if (m_progress != null)
                {
                    return m_progress.Value;
                }
                else
                {
                    return 0.0;
                }
            }

            private set
            {
                var time = (CurrentDevice as Models.IBoseSoundTouchDevice).TrackInfo.time;
                var total = (CurrentDevice as Models.IBoseSoundTouchDevice).TrackInfo.timeTotal;
                var progress = time.TotalSeconds / total.TotalSeconds * 100.0;
                if (progress != m_progress)
                {
                    m_progress = progress;
                    OnPropertyChanged();
                }
            }
        }

        private bool m_progressApplicable = default(bool);
        [Device(CURRENT)]
        public bool ProgressApplicable
        {
            get
            {
                return m_progressApplicable;
            }

            private set
            {
                var progressApplicable = TimeSpan.Zero != (CurrentDevice as Models.IBoseSoundTouchDevice).TrackInfo.timeTotal;
                if (progressApplicable != m_progressApplicable)
                {
                    m_progressApplicable = progressApplicable;
                    OnPropertyChanged();
                }
            }
        }

        private Uri m_playState = default(Uri);

        [Device(CURRENT)]
        public Uri PlayState
        {
            get
            {
                return m_playState;
            }

            private set
            {
                var state = (CurrentDevice as Models.IBoseSoundTouchDevice).TrackInfo.playStatus;
                var source = "ms-appx://" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "/Assets/";
                switch (state)
                {
                    case PLAY_STATUS.PLAY_STATE:
                        source += "Play.png";
                        break;
                    case PLAY_STATUS.PAUSE_STATE:
                        source += "Pause.png";
                        break;
                    case PLAY_STATUS.STOP_STATE:
                        source += "Stop.png";
                        break;
                    default:
                        source += "Stop.png";
                        break;
                }

                Uri playState = null;
                if (Uri.IsWellFormedUriString(source, UriKind.RelativeOrAbsolute))
                {
                    playState = new Uri(source);
                }

                if (playState != m_playState)
                {
                    m_playState = playState;
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
    }
}
