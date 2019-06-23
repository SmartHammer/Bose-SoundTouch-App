using BoseSoundTouchApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BoseSoundTouchApp.Services
{
    public class POST_volume : POST
    {
        public POST_volume(IPhysicalData physicalData)
            : base(physicalData)
        {
        }

        public int Volume
        {
            set
            {
                POST.Node node = new POST.Node("volume", value.ToString());
                Post(node);
            }
        }
    }
}
