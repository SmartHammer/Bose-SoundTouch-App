using System.Collections.Generic;

namespace BoseSoundTouchApp.Models
{
    public interface ISourceInfo
    {
        // comes from now_playing.source
        string source
        {
            get;
        }

        // comes from now_playing.ContentItem.itemName
        string sourceName
        {
            get;
        }

        // comes from now_playing.ContentItem.containerArt
        string sourceArt
        {
            get;
        }
    }
}
