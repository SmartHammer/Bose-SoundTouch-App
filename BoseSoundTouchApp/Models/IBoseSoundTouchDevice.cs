﻿using System;
using System.Collections.Generic;

namespace BoseSoundTouchApp.Models
{
    public interface IBoseSoundTouchDevice : IDevice
    {
        IDeviceState State
        {
            get;
            set;
        }

        IVolume Volume
        {
            get;
            set;
        }

        IPresets Presets
        {
            get;
        }

        ITrackInfo TrackInfo
        {
            get;
        }

        ISourceInfo SourceInfo
        {
            get;
        }

        void Initialize();

        void SelectPreset(int index);
    }
}
