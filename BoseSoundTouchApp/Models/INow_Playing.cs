using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoseSoundTouchApp.Models
{
    // The state of an image
    public enum ART_STATUS
    {
        INVALID,
        SHOW_DEFAULT_IMAGE,
        DOWNLOADING,
        IMAGE_PRESENT
    }

    // The state of the audio stream
    public enum PLAY_STATUS
    {
        PLAY_STATE,
        PAUSE_STATE,
        STOP_STATE,
        BUFFERING_STATE
    }

    // The state of shuffle.
    public enum SHUFFLE_STATUS
    {
        SHUFFLE_OFF,
        SHUFFLE_ON
    }

    // The state of repeat.
    public enum REPEAT_STATUS
    {
        REPEAT_OFF,
        REPEAT_ALL,
        REPEAT_ONE
    }

    // The type of music container that is streaming.
    public enum STREAM_STATUS
    {
        TRACK_ONDEMAND,
        RADIO_STREAMING,
        RADIO_TRACKS,
        NO_TRANSPORT_CONTROLS
    }

    public interface INow_Playing
    {
        List<string> dependsOn
        {
            get;
        }

        //Attribute. Unique identifier of the product.
        string deviceID
        {
            get;
        }

        //Attribute. The type or name of the service playing. To determine if the product is in standby mode, check if source == STANDBY.
        string source
        {
            get;
        }

        //Attribute. The account associated with this source.
        string sourceAccount
        {
            get;
        }

        //Element. ContentItem.ItemName (e.g. Folder on server, station of internet radio, device name of BLUETOOTH)
        string ContentItemName
        {
            get;
        }

        //Element. ContentItem.containerArt (e.g. icon of internet radios station)
        string ContentItemContainerArt
        {
            get;
        }

        //Element. The track name.
        string track
        {
            get;
        }

        //Element. The artist name.
        string artist
        {
            get;
        }

        //Element. The album name.
        string album
        {
            get;
        }

        //Element. The music genre.
        string genre
        {
            get;
        }

        //Element. The user rating of the song.
        string userRating
        {
            get;
        }

        //Element. The station or playlist name.
        string stationName
        {
            get;
        }

        //The URL of the source art.
        string art
        {
            get;
        }

        //Indicates whether or not an image is available.
        ART_STATUS artImageStatus
        {
            get;
        }

        //The length of the track, in seconds.
        int time
        {
            get;
        }

        //Tells you the current time the track has played, in seconds.
        int timeTotal
        {
            get;
        }

        //Element. This indicates whether the audio product is currently playing, paused, in standby, or in some other state.
        PLAY_STATUS playStatus
        {
            get;
        }

        //Element. The type of music that is streaming.
        STREAM_STATUS streamType
        {
            get;
        }

        //Element. The location of the source ex: "Internet Only".
        string stationLocation
        {
            get;
        }

        //This object describes the connection status to the Bluetooth source.
        string ConnectionStatus
        {
            get;
        }

        //The name of the Bluetooth source.
        string ConnectionStatusDeviceName
        {
            get;
        }
    }
}
