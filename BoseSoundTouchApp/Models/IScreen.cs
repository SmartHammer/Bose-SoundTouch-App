using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoseSoundTouchApp.Models
{
    public interface IScreen
    {
        double Brightness
        {
            set;
        }

        void Dimming(bool down = true);
    }
}
