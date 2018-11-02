using BoseSoundTouchApp.Bases;
using BoseSoundTouchApp.Models;
using System;
using System.Collections.Generic;

namespace BoseSoundTouchApp.ViewModels
{
    public enum DeviceType
    {
        MediaServer,
        BoseSoundTouchDevice,
        MediaRenderer
    }

    public class InfoPageViewModel : NotiferClass
    {
        #region Members
        private string m_title;
        private string m_footer;
        private List<DeviceType> m_foundDevices;
        private Models.IGeneralModel m_model;
        #endregion Members

        public InfoPageViewModel()
        {
            m_title = "SoundTouch APP";
            m_footer = "Suche nach SoundTouch Geräten";
            m_foundDevices = new List<DeviceType>{};
        }

        public string Title
        {
            get
            {
                return m_title;
            }
            set
            {
                m_title = value;
                OnPropertyChanged();
            }
        }

        public string Footer
        {
            get
            {
                return m_footer;
            }

            set
            {
                m_footer = value;
                OnPropertyChanged();
            }
        }

        public List<DeviceType> FoundDevices
        {
            get
            {
                return m_foundDevices;
            }

            set
            {
                lock (m_foundDevices)
                {
                    m_foundDevices = value;
                }
                OnPropertyChanged();
            }
        }

        public Models.IGeneralModel Model
        {
            set
            {
                m_model = value;
                (m_model as Models.GeneralModel).PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler((o, e) =>
                {
                    switch (e.PropertyName)
                    {
                        case "Devices":
                            lock (m_model.Devices)
                            {
                                m_foundDevices.Clear();
                                m_foundDevices = m_model.Devices.ConvertAll<DeviceType>(new Converter<Models.IDevice, DeviceType>(p => p.IsServer ? DeviceType.MediaServer : p is BoseSoundTouchDevice ? DeviceType.BoseSoundTouchDevice : DeviceType.MediaRenderer));
                            }

                            OnPropertyChanged("FoundDevices");
                            break;
                    }
                });
            }
        }
    }
}
