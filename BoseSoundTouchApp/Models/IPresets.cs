using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoseSoundTouchApp.Models
{
    public interface IPreset
    {
        //Attribute. ContentItem.source (e.g. TUNEIN, etc)
        string Source
        {
            get;
        }

        //Element. ContentItem.ItemName (e.g. Folder on server, station of internet radio, device name of BLUETOOTH)
        string ContentItemName
        {
            get;
        }

        //Element. ContentItem.containerArt (e.g. icon of internet radios station)
        string ContentItemContainerArt
        {
            get;
        }
    }

    public interface IPresets
    {
        Dictionary<int, IPreset> Presets
        {
            get;
        }
    }
}
