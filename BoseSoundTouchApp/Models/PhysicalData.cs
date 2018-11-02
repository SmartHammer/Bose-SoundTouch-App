using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BoseSoundTouchApp.Bases;

namespace BoseSoundTouchApp.Models
{
    internal class PhysicalData : NotiferClass, IPhysicalData
    {
        #region Members
        private IPAddress m_ipAddress;
        private DeviceType m_deviceType;
        private string m_xmlDescriptionPath;
        #endregion Members

        public PhysicalData(IPAddress iPAddress, DeviceType deviceType, string xmlDescriptionPath)
        {
            m_ipAddress = iPAddress;
            m_deviceType = deviceType;
            m_xmlDescriptionPath = xmlDescriptionPath;
        }

        #region Properties
        /// <see>
        /// IPhysicalData.Address
        /// </see>
        public IPAddress Address
        {
            get
            {
                return m_ipAddress;
            }
            set
            {
                m_ipAddress = value;
                OnPropertyChanged();
            }
        }

        /// <see>
        /// IPhysicalData.Type
        /// </see>
        public DeviceType Type
        {
            get
            {
                return m_deviceType;
            }
            set
            {
                m_deviceType = value;
                OnPropertyChanged();
            }
        }

        /// <see>
        /// IPhysicalData.XmlDescriptionPath
        /// </see>
        public string XmlDescriptionPath
        {
            get
            {
                return m_xmlDescriptionPath;
            }
            set
            {
                m_xmlDescriptionPath = value;
                OnPropertyChanged();
            }
        }
        #endregion Properties
    }
}
