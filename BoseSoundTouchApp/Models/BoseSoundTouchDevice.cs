using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BoseSoundTouchApp.Models
{
    public class BoseSoundTouchDevice : Device, IBoseSoundTouchDevice
    {
        #region Members
        private readonly CancellationTokenSource m_cancelToken;
        private readonly Dictionary<Type, object> m_services;
        private List<object> m_updateServices;
        #endregion Members

        public BoseSoundTouchDevice(IPhysicalData physicalData, XDocument xDoc)
            : base(physicalData, xDoc)
        {
            m_services = new Dictionary<Type, object>();
            RegisterService(typeof(Services.GET_now_playing));
            RegisterService(typeof(Services.GET_volume));
            RegisterService(typeof(Services.GET_presets));
            m_cancelToken = new CancellationTokenSource();
            m_updateServices = new List<object>();
            PrepareUpdater();
        }

        private void PrepareUpdater()
        {
            var properities = GetType().GetProperties(
                BindingFlags.DeclaredOnly |
                BindingFlags.GetProperty |
                BindingFlags.Instance |
                BindingFlags.Public);
            List<string> requiredProps = new List<string>();
            foreach (var prop in properities)
            {
                var typeProps = prop.PropertyType.GetProperties(
                    BindingFlags.GetProperty |
                    BindingFlags.Instance |
                    BindingFlags.Public);
                foreach (var typeProp in typeProps)
                {
                    requiredProps.Add(typeProp.Name);
                }
            }

            foreach (var service in m_services)
            {
                if (service.Value is Services.GET)
                {
                    var props = (service.Value as Services.GET).GetPropertyNames();
                    bool updateRequired = false;
                    foreach(var requiredProp in requiredProps)
                    {
                        if (props.Contains(requiredProp))
                        {
                            m_updateServices.Add(service.Value);
                            updateRequired = true;
                        }
                    }

                    if (updateRequired)
                    {
                        (service.Value as Services.GET).PropertyChanged += Notifier;
                    }
                }
            }

            m_updateServices = m_updateServices.Distinct().ToList();
        }

        #region Properties
        [Dependency("GET_volume")]
        public IVolume Volume
        {
            get
            { 
                var service = (GetService(typeof(Services.GET_volume)) as Services.GET_volume);
                return new Volume(service.deviceID, service.targetvolume, service.actualvolume, service.muteenabled);
            }
            set
            {
                Services.POST_volume service = new Services.POST_volume(PhysicalData)
                {
                    Volume = value.targetvolume
                };
            }
        }

        [Dependency("GET_presets")]
        public IPresets Presets
        {
            get
            {
                var service = (GetService(typeof(Services.GET_presets)) as Services.GET_presets);
                return new DevicePresets(service.Presets);
            }
        }

        [Dependency("GET_now_playing")]
        public ITrackInfo TrackInfo
        {
            get
            {
                var service = (GetService(typeof(Services.GET_now_playing)) as Services.GET_now_playing);
                return new TrackInfo(service.track, service.album, service.artist, service.art, service.artImageStatus, service.genre, service.playStatus, service.offset, service.time, service.timeTotal);
            }
        }

        [Dependency("GET_now_playing")]
        public ISourceInfo SourceInfo
        {
            get
            {
                var service = (GetService(typeof(Services.GET_now_playing)) as Services.GET_now_playing);
                return new SourceInfo(service.source, service.ContentItemName, service.ContentItemContainerArt);
            }
        }

        [Dependency("GET_now_playing")]
        public IDeviceState State
        {
            get
            {
                var service = (GetService(typeof(Services.GET_now_playing)) as Services.GET_now_playing);
                return new DeviceState(service.source == "STANDBY");
            }
            set
            {
                Services.POST_key service = new Services.POST_key(PhysicalData)
                {
                    Key = new Services.POST_key.KeyType(Services.POST_key.KeyType.Keys.POWER, Services.POST_key.KeyType.States.PRESS)
                };
                service.Key = new Services.POST_key.KeyType(Services.POST_key.KeyType.Keys.POWER, Services.POST_key.KeyType.States.RELEASE);
            }
        }
        #endregion Properties

        #region Methods
        public object GetService(Type serviceType)
        {
            object result = null;
            if (m_services.ContainsKey(serviceType))
            {
                result = m_services[serviceType];
            }

            return result;
        }

        public void SelectPreset(int index)
        {
            Services.POST_key service = new Services.POST_key(PhysicalData)
            {
                Key = new Services.POST_key.KeyType(Services.POST_key.KeyType.Keys.PRESET_1 + index - 1, Services.POST_key.KeyType.States.RELEASE)
            };
        }

        public void Initialize()
        {
            StartWorker();
        }

        public new void Dispose()
        {
            m_cancelToken.Cancel();
        }

        private void RegisterService(Type serviceType)
        {
            if (!m_services.ContainsKey(serviceType))
            {
                m_services.Add(serviceType, System.Activator.CreateInstance(type: serviceType, args: new object[] { this.PhysicalData }));
            }
        }

        private void Notifier(object sender, PropertyChangedEventArgs e)
        {
            string propertyName = string.Empty;
            string getter = string.Empty;
            var parts = e.PropertyName.Split('.');
            if (parts.Count() > 0)
            {
                propertyName += parts[parts.Count() - 1];
                getter = parts[0];
            }
            else
            {
                propertyName += e.PropertyName;
            }

            if (propertyName == "Document")
            {
                var properities = GetType().GetProperties(
                    BindingFlags.DeclaredOnly |
                    BindingFlags.GetProperty |
                    BindingFlags.Instance |
                    BindingFlags.Public);
                foreach (var prop in properities)
                {
                    // Using reflection.  
                    System.Attribute[] attrs = Attribute.GetCustomAttributes(prop);  // Reflection.  
                    var depAttrs = from attr in attrs
                                   where attr is DependencyAttribute
                                   select attr as DependencyAttribute;
                    foreach(var depAttr in depAttrs)
                    {
                        if (depAttr.DependsOn == getter)
                        {
                            OnPropertyChanged(UDN + "." + prop.Name);
                        }
                    }
                }
            }
        }
        #endregion Methods

        #region Worker
        private void StartWorker()
        {
            var maxCycleTimeInMS = (m_updateServices.OrderByDescending(item => (item as Services.GET).CycleTimeInMS).First() as Services.GET).CycleTimeInMS;
            var cycleTimeInMS = maxCycleTimeInMS;
            foreach (var service in m_updateServices)
            {
                var cycle = (service as Services.GET).CycleTimeInMS;
                var max = Math.Max(cycleTimeInMS, cycle);
                var min = Math.Min(cycleTimeInMS, cycle);
                while (0 != (max % min))
                {
                    var remaining = max % min;
                    max = min;
                    min = remaining;
                }

                cycleTimeInMS = min;
            }

            TimeSpan time = TimeSpan.FromMilliseconds(cycleTimeInMS);
            Task.Factory.StartNew(async () =>
            {
                var cycleTime = time;
                while (true)
                {
                    try
                    {
                        foreach (var service in m_updateServices)
                        {
                            if (cycleTime.TotalMilliseconds == (service as Services.GET).CycleTimeInMS)
                            {
                                (service as Services.GET).Update();
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }

                    await Task.Delay(time);
                    cycleTime += time;
                    if (cycleTime.TotalMilliseconds > maxCycleTimeInMS)
                        cycleTime = time;
                }
            }
            , m_cancelToken.Token);
        }
        #endregion Worker
    }
}
