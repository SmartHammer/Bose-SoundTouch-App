using BoseSoundTouchApp.Models;

namespace BoseSoundTouchApp.Services
{
    public class GET_sources : GET, ISources
    {
        public GET_sources(ref IPhysicalData physicalData)
            : base(ref physicalData)
        { }

        public new int CycleTimeInMS => 2000;

        public string deviceID
        {
            get
            {
                return Supporter.GetAttribute<string>(Document, "sources");
            }
        }

        public string sourceItem
        {
            get
            {
                return Supporter.GetValue<string>(Document);
            }
        }

        public string source
        {
            get
            {
                return Supporter.GetAttribute<string>(Document, "sourceItem");
            }
        }

        public string sourceAccount
        {
            get
            {
                return Supporter.GetAttribute<string>(Document, "sourceItem");
            }
        }

        public SOURCE_STATUS status
        {
            get
            {
                return Supporter.GetAttribute<SOURCE_STATUS>(Document, "sourceItem");
            }
        }

        public bool isLocal
        {
            get
            {
                return Supporter.GetAttribute<bool>(Document, "sourceItem");
            }
        }

        public bool multiroomallowed
        {
            get
            {
                return Supporter.GetAttribute<bool>(Document, "sourceItem");
            }
        }
    }
}
