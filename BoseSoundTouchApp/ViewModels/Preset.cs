using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BoseSoundTouchApp.ViewModels
{
    [DataContract]
    public class Preset
    {
        [DataMember]
        public uint Number
        {
            get;
            set;
        }

        [DataMember]
        public Uri Icon
        {
            get;
            set;
        }

        [DataMember]
        public string Name
        {
            get;
            set;
        }

        [DataMember]
        public Uri Type
        {
            get;
            set;
        }

        [DataMember]
        public string TypeName
        {
            get;
            set;
        }
    }
}
