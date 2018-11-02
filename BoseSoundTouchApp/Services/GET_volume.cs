using BoseSoundTouchApp.Models;

namespace BoseSoundTouchApp.Services
{
    public class GET_volume : GET
    {
        public GET_volume(ref IPhysicalData physicalData)
            : base(ref physicalData)
        { }

        public new int CycleTimeInMS => 500;

        public int targetvolume
        {
            get
            {
                return Supporter.GetValue<int>(Document);
            }
        }


        public int actualvolume
        {
            get
            {
                return Supporter.GetValue<int>(Document);
            }
        }

        public bool muteenabled
        {
            get
            {
                return Supporter.GetValue<bool>(Document);
            }
        }

        public string deviceID
        {
            get
            {
                return Supporter.GetAttribute<string>(Document, "volume");
            }
        }
    }
}
