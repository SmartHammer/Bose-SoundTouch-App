namespace BoseSoundTouchApp.Models
{
    public class Preset : IPreset
    {
        public Preset(string source, string itemName, string containerArt)
        {
            Source = source;
            ContentItemName = itemName;
            ContentItemContainerArt = containerArt;
        }

        public string Source
        {
            get;
            private set;
        }

        public string ContentItemName
        {
            get;
            private set;
        }

        public string ContentItemContainerArt
        {
            get;
            private set;
        }
    }
}
