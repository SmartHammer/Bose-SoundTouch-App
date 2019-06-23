using BoseSoundTouchApp.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BoseSoundTouchApp.Models
{
    public class DeviceSearcher : NotiferClass, IDeviceSearcher
    {
        #region CONST
        private const int TimeoutSeconds = 3;
        public enum SearchingWhat
        {
            MediaServer,
            MediaRenderer
        }
        #endregion CONST

        #region Members
        private CancellationTokenSource m_cancelToken;
        #endregion Members

        public DeviceSearcher()
        {
            m_cancelToken = new CancellationTokenSource(new TimeSpan(0, 0, TimeoutSeconds));
            Devices = new List<IDevice>();
        }

        public List<IDevice> Devices
        {
            get;
            private set;
        }

        public void Start()
        {
            Devices.Clear();
            StartWorker();
        }

        public void Stop()
        {
            m_cancelToken.Cancel();
        }

        public event DeviceSearcherFinished.Handler SearchFinished;

        #region Worker
        private void StartWorker()
        {
            Task.Factory.StartNew(async () =>
            {
                while (Devices.Count() <= 0)
                {
                    IPEndPoint LocalEndPoint = new IPEndPoint(address: IPAddress.Any, port: 0);
                    IPEndPoint MulticastEndPoint = new IPEndPoint(address: IPAddress.Parse("239.255.255.250"), port: 1900);
                    Request command = new Request(
                        method: "M-SEARCH",
                        uniformResourceIdentifier: "*",
                        httpProtocolVersion: "HTTP/1.1",
                        host: MulticastEndPoint,
                        mandatoryExtension: "ssdp:discover",
                        searchTarget: "ssdp:all",
                        maximumWait: TimeoutSeconds
                    );
                    using (Socket UdpSocket = new Socket(addressFamily: AddressFamily.InterNetwork, socketType: SocketType.Dgram, protocolType: ProtocolType.Udp))
                    {
                        UdpSocket.Bind(LocalEndPoint);
                        lock (UdpSocket)
                        {
                            UdpSocket.SendTo(buffer: command,
                                                socketFlags: SocketFlags.None,
                                                remoteEP: MulticastEndPoint);
                        }
                        Timer watchdog = new Timer(WatchdogElapsedHandler, null, 10000 * TimeoutSeconds, Timeout.Infinite);
                        Timer timer = null;
                        while (true)
                        {
                            if (timer != null)
                            {
                                timer.Dispose();
                            }

                            timer = new Timer(TimerElapsedHandler, null, 1000 * TimeoutSeconds, Timeout.Infinite);
                            byte[] buffer = new byte[64000];
                            int received = UdpSocket.Receive(buffer, buffer.Length, SocketFlags.None);
                            watchdog.Dispose();
                            timer.Dispose();
                            if (received > 0)
                            {
                                Response response = new Response(buffer, received);
                                if (response.Successful)
                                {
                                    if (new List<DeviceType> { DeviceType.MediaRenderer, DeviceType.MediaServer }.Contains(response.DeviceType))
                                    {
                                        if (response.XMLDescriptionValid)
                                        {
                                            IPhysicalData physicalData = new PhysicalData(response.IPAddress, response.DeviceType, response.XMLDescription);
                                            lock (Devices)
                                            {
                                                if (Devices.Find(item => item.PhysicalData.Address == physicalData.Address) == null)
                                                {
                                                    var device = Device.ReadLocation(physicalData);
                                                    Devices.Add(device);
                                                    OnPropertyChanged("Devices");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(30.0));
            },
            m_cancelToken.Token);
        }

        private void TimerElapsedHandler(object state)
        {
            m_cancelToken.Cancel();
            var args = new DeviceSearcherEventArgs(Devices.Count() > 0);
            SearchFinished(this, args);
        }

        private void WatchdogElapsedHandler(object state)
        {
            m_cancelToken.Cancel();
            var args = new DeviceSearcherEventArgs(Devices.Count() > 0);
            SearchFinished(this, args);
        }
        #endregion Worker
    }
}
