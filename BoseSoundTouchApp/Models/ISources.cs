using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoseSoundTouchApp.Models
{
    // The availability of an audio source
    public enum SOURCE_STATUS
    {
        UNAVAILABLE,
        READY
    }

    public interface ISources
    {
        // Attribute. Unique identifier of the product.
        string deviceID
        { get; }

        // The user-facing name of the source.
        string sourceItem
        { get; }

        // Attribute. The name of the source.
        string source
        { get; }

        // Attribute. The user account associated with the source.
        string sourceAccount
        { get; }

        // Attribute. Indicates whether the source is available.
        SOURCE_STATUS status
        { get; }

        // Attribute. TRUE if a local source (AUX or BLUETOOTH)
        bool isLocal
        { get; }

        // Attribute. TRUE if the source can be rebroadcast in a multi-room zone.
        bool multiroomallowed
        { get; }
}
}
