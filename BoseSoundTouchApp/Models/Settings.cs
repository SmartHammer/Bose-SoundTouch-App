using System;
using Windows.Storage;
using BoseSoundTouchApp.ViewModels;

namespace BoseSoundTouchApp.Models
{
    public class Settings
    {
        public const string Path = "Settings.xml";

        public Settings()
        {
            UDN = "";
        }

        public string UDN
        {
            get;
            set;
        }

        public static implicit operator Settings(ApplicationDataCompositeValue values)
        {
            var settings = new Settings();

            var udn = values["UDN"] as string;
            settings.UDN = udn;
            return settings;
        }

        public static implicit operator ApplicationDataCompositeValue(Settings settings)
        {
            var values = new ApplicationDataCompositeValue();
            values["UDN"] = settings.UDN;
            return values;
        }

        public static bool equals(ApplicationDataCompositeValue settings, IDevice device)
        {
            return settings["UDN"] as string == device.UDN;
        }
    }
}
