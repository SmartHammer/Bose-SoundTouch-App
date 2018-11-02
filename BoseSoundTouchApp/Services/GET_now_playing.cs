using System;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using Windows.UI.Xaml.Controls;
using BoseSoundTouchApp.Models;

namespace BoseSoundTouchApp.Services
{
    public class GET_now_playing : GET
    {
        public GET_now_playing(ref Models.IPhysicalData physicalData)
            : base(ref physicalData)
        {
        }

        public string deviceID
        {
            get
            {
                return Supporter.GetAttribute<string>(Document, "nowPlaying");
            }
        }

        public string source
        {
            get
            {
                return Supporter.GetAttribute<string>(Document, "nowPlaying");
            }
        }

        public string ContentItemName
        {
            get
            {
                return Supporter.GetValue<string>(Document, "itemName");
            }
        }

        public string ContentItemContainerArt
        {
            get
            {
                return Supporter.GetValue<string>(Document, "containerArt");
            }
        }

        public string track
        {
            get
            {
                return Supporter.GetValue<string>(Document);
            }
        }

        public string artist
        {
            get
            {
                return Supporter.GetValue<string>(Document);
            }
        }

        public string album
        {
            get
            {
                return Supporter.GetValue<string>(Document);
            }
        }

        public int offset
        {
            get
            {
                return Supporter.GetValue<int>(Document);
            }
        }

        public int time
        {
            get
            {
                return Supporter.GetValue<int>(Document);
            }
        }

        public int timeTotal
        {
            get
            {
                return Supporter.GetAttribute<int>(Document, "time", "total");
            }
        }

        public string stationName
        {
            get
            {
                return Supporter.GetValue<string>(Document);
            }
        }

        public string art
        {
            get
            {
                return Supporter.GetValue<string>(Document);
            }
        }

        public ART_STATUS artImageStatus
        {
            get
            {
                Enum.TryParse(Supporter.GetAttribute<string>(Document, "art"), out ART_STATUS status);
                return status;
            }
        }

        public PLAY_STATUS playStatus
        {
            get
            {
                Enum.TryParse(Supporter.GetValue<string>(Document), out PLAY_STATUS status);
                return status;
            }
        }

        public string genre
        {
            get
            {
                return Supporter.GetValue<string>(Document);
            }
        }

        public string ConnectionStatus
        {
            get
            {
                return Supporter.GetAttribute<string>(Document, "connectionStatusInfo", "status");
            }
        }

        public string ConnectionStatusDeviceName
        {
            get
            {
                return Supporter.GetAttribute<string>(Document, "connectionStatusInfo", "deviceName");
            }
        }

        public string connectedDevice
        {
            get
            {
                return Supporter.GetAttribute<string>(Document, "connectionStatusInfo", "deviceName");
            }
        }

        public string sourceAccount
        {
            get
            {
                return Supporter.GetAttribute<string>(Document, "nowPlaying");
            }
        }

        public string userRating
        {
            get
            {
                return Supporter.GetValue<string>(Document, "rating");
            }
        }

        public STREAM_STATUS streamType
        {
            get
            {
                return Supporter.GetValue<STREAM_STATUS>(Document);
            }
        }

        public string stationLocation
        {
            get
            {
                return Supporter.GetValue<string>(Document);
            }
        }
    }
}
