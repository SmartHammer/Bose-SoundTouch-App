using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoseSoundTouchApp.Models
{
    public class Volume : IVolume
    {
        public Volume(string deviceID, int targetvolume, int actualvolume, bool muteenabled)
        {
            dependsOn = new List<string> { "GET_volume" };
            this.deviceID = deviceID;
            this.targetvolume = targetvolume;
            this.actualvolume = actualvolume;
            this.muteenabled = muteenabled;
        }

        public List<string> dependsOn
        {
            get;
            private set;
        }

        public string deviceID
        {
            get;
            private set;
        }

        public int targetvolume
        {
            get;
        }

        public int actualvolume
        {
            get;
            private set;
        }

        public bool muteenabled
        {
            get;
            private set;
        }
    }
}
