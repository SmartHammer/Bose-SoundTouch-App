using System.Collections.Generic;

namespace BoseSoundTouchApp.Models
{
    public interface IVolume
    {
        // Attribute. Unique identifier of the product.
        string deviceID
        {
            get;
        }

        // Element. The volume the product is trying to reach, 0 to 100 inclusive. Bigger is louder.
        int targetvolume
        {
            get;
        }

        // Element. The current product volume, 0 to 100 inclusive. Bigger is louder.
        int actualvolume
        {
            get;
        }

        // Element. TRUE if the product is muted.
        bool muteenabled
        {
            get;
        }
    }
}
