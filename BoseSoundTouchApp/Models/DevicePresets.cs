using System.Collections.Generic;

namespace BoseSoundTouchApp.Models
{
    public class DevicePresets : IPresets
    {
        public DevicePresets(Dictionary<int, IPreset> presets)
        {
            dependsOn = new List<string> { "GET_presets" };
            Presets = presets;
        }

        public List<string> dependsOn
        {
            get;
            private set;
        }

        public Dictionary<int, IPreset> Presets
        {
            get;
            private set;
        }
    }
}
