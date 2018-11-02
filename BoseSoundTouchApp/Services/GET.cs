using BoseSoundTouchApp.Bases;
using BoseSoundTouchApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BoseSoundTouchApp.Services
{
    abstract public class GET : NotiferClass
    {
        #region Members
        private IPhysicalData m_physicalData;
        #endregion Members

        public GET(ref IPhysicalData physicalData)
        {
            m_physicalData = physicalData;
        }

        public void Update()
        {
            var document = Supporter.Get(ipAddress: m_physicalData.Address,
                                            serviceName: GetType().Name.Substring("GET_".Length));
            if (null == document) return;  // hack for presets that sometimes are not sent completely and thus return null
            if (!compare(document, Document))
            {
                var previous = System.Activator.CreateInstance(type: this.GetType(), args: new object[] { this.m_physicalData });
                (previous as GET).Document = Document;
                Document = document;
                var properties = this.GetType().GetProperties(
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.GetProperty);
                if (this is Services.GET_now_playing)
                {
                    foreach (var prop in properties)
                    {
                        var propName = prop.Name;
                    }
                }
                foreach (var property in properties)
                {
                    try
                    {
                        if (property.GetValue(previous) != property.GetValue(this))
                        {
                            OnPropertyChanged(GetType().Name + "." + property.Name);
                        }
                    }
                    catch (Exception)
                    {
                        if (Document != null)
                        {
                            OnPropertyChanged(GetType().Name + "." + property.Name);
                        }
                    }
                }
            }
        }

        public XDocument Document
        {
            get;
            private set;
        }

        public virtual int CycleTimeInMS => 1000;

        public List<string> GetPropertyNames()
        {
            List<string> result = new List<string>();
            var properties = this.GetType().GetProperties(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.GetProperty);
            foreach(var prop in properties)
            {
                result.Add(prop.Name);
            }

            return result;
        }

        public bool RelevantForUpdate(List<string> propertyNames)
        {
            var properties = this.GetType().GetProperties(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.GetProperty);
            bool relevant = true;
            relevant &= propertyNames.Any(prop => (from p in properties where p.Name == prop select prop).Count() > 0);
            return relevant;
        }

        private bool compare(XDocument first, XDocument second)
        {
            bool result = (first == null && second == null);
            if ( first != null && second != null)
            {
                result = XDocument.DeepEquals(first, second);
            }

            return result;
        }
    }
}
