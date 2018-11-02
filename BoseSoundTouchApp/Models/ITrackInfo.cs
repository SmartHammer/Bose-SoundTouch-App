using System;
using System.Collections.Generic;

namespace BoseSoundTouchApp.Models
{
    public interface ITrackInfo
    {
        // comes from now_playing.track
        string track
        {
            get;
        }
        // comes from now_playing.album
        string album
        {
            get;
        }
        // comes from now_playing.artist
        string artist
        {
            get;
        }
        // comes from now_playing.art
        string art
        {
            get;
        }
        // comes from now_playing.artImageStatus
        ART_STATUS art_status
        {
            get;
        }
        // comes from now_playing.genre
        string genre
        {
            get;
        }
        // comes from now_playing.offset
        int offset
        {
            get;
        }
        // comes from now_playing.time
        TimeSpan time
        {
            get;
        }
        // comes from now_playing.time
        TimeSpan timeTotal
        {
            get;
        }
        // comes from now_playing.playStatus
        PLAY_STATUS playStatus
        {
            get;
        }
    }
}
