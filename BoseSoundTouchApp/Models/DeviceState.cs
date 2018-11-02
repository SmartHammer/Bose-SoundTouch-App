using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoseSoundTouchApp.Models
{
    public class DeviceState : IDeviceState
    {
        public DeviceState(bool standBy)
        {
            dependsOn = new List<string> { "GET_now_playing" };
            this.standBy = standBy;
        }

        public List<string> dependsOn
        {
            get;
            private set;
        }

        public bool standBy
        {
            get;
            private set;
        }
    }
}
