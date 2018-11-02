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
            var value = (CurrentDevice as Models.IBoseSoundTouchDevice).TrackInfo.track;
            if ( ! string.IsNullOrEmpty(value))
            {
                if ((value.Length > 15) && value.Contains(" ("))
                {
                    value = value.Replace(" (", "\n(");
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

        private string album = default(string);
        private void SetAlbum()
        {
            var value = (CurrentDevice as Models.IBoseSoundTouchDevice).TrackInfo.album;

            if (value != album)
            {
                album = value;
                OnPropertyChanged("Album");
            }
        }

        [Device(CURRENT)]
        public string Album
        {
            get
            {
                return album;
            }
        }

        private string currentTime = default(string);
        private void SetCurrentTime()
        {
            var time = (CurrentDevice as Models.IBoseSoundTouchDevice).TrackInfo.time.ToString(@"mm\:ss");
            if (currentTime != time)
            {
                currentTime = time;
                OnPropertyChanged("CurrentTime");
            }
        }

        [Device(CURRENT)]
        public string CurrentTime
        {
            get
            {
                return currentTime;
            }
        }

        private string totalTime = default(string);
        private void SetTotalTime()
        {
            var time = (CurrentDevice as Models.IBoseSoundTouchDevice).TrackInfo.timeTotal.ToString(@"mm\:ss");
            if (totalTime != time)
            {
                totalTime = time;
                OnPropertyChanged("TotalTime");
            }
        }

        [Device(CURRENT)]
        public string TotalTime
        {
            get
            {
                return totalTime;
            }
        }

        private double? progress = default(double?);
        private void SetProgress()
        {
            var time = (CurrentDevice as Models.IBoseSoundTouchDevice).TrackInfo.time;
            var total = (CurrentDevice as Models.IBoseSoundTouchDevice).TrackInfo.timeTotal;
            var value = time.TotalSeconds / total.TotalSeconds * 100.0;
            if (value != progress)
            {
                progress = value;
                OnPropertyChanged("Progress");
            }
        }

        [Device(CURRENT)]
        public double Progress
        { 
            get
            {
                if (progress != null)
                {
                    return progress.Value;
                }
                else
                {
                    return 0.0;
                }
            }
        }
        private Uri playState = default(Uri);
        private void SetPlayState()
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

            Uri value = null;
            if (Uri.IsWellFormedUriString(source, UriKind.RelativeOrAbsolute))
            {
                value = new Uri(source);
            }

            if (value != playState)
            {
                playState = value;
                OnPropertyChanged("PlayState");
            }
        }

        [Device(CURRENT)]
        public Uri PlayState
        {
            get
            {
                return playState;
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
    }
}
