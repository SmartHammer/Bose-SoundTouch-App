using System.Collections.Generic;
using BoseSoundTouchApp.Models;

namespace BoseSoundTouchApp.Services
{
    public class GET_getZone : GET, IGetZone
    {
        public GET_getZone(ref IPhysicalData physicalData)
            : base(ref physicalData)
        { }

        public string master
        {
            get
            {
                return Supporter.GetAttribute<string>(Document, "getZone");
            }
        }

        public string senderIPAddress
        {
            get
            {
                return Supporter.GetValue<string>(Document);
            }
        }

        public bool senderIsMaster
        {
            get
            {
                return Supporter.GetValue<bool>(Document);
            }
        }

        public List<IMember> member
        {
            get
            {
                return new List<IMember>();
            }
        }
    }
}
