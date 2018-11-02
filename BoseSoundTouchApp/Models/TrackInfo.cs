using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoseSoundTouchApp.Models
{
    public class TrackInfo : ITrackInfo
    {
        public TrackInfo(string track, string album, string artist, string art, ART_STATUS art_status, string genre, PLAY_STATUS playStatus, int offset, int time, int timeTotal)
        {
            dependsOn = new List<string> { "GET_now_playing" };
            this.track = track;
            this.album = album;
            this.art = art;
            this.art_status = art_status;
            this.artist = artist;
            this.genre = genre;
            this.offset = offset;
            this.playStatus = playStatus;
            this.time = new TimeSpan(0,0,time);
            this.timeTotal = new TimeSpan(0,0,timeTotal);
        }

        public List<string> dependsOn
        {
            get;
            private set;
        }

        public string track
        {
            get;
            private set;
        }

        public string album
        {
            get;
            private set;
        }

        public string artist
        {
            get;
            private set;
        }

        public string art
        {
            get;
            private set;
        }

        public ART_STATUS art_status
        {
            get;
            private set;
        }

        public string genre
        {
            get;
            private set;
        }

        public int offset
        {
            get;
            private set;
        }

        public TimeSpan time
        {
            get;
            private set;
        }

        public TimeSpan timeTotal
        {
            get;
            private set;
        }

        public PLAY_STATUS playStatus
        {
            get;
            private set;
        }
    }
}
