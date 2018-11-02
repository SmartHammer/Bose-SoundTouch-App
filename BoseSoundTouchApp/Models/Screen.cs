using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BoseSoundTouchApp.Bases;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace BoseSoundTouchApp.Models
{
    public class Screen : NotiferClass, IScreen
    {
        private const int TimeoutSeconds = 1;
        private const int DimmingCycleTimeInMS = 100;
        private I2cDevice m_i2CDevice;
        private const double DefaultBrightness = 100;
        private double currentBrightness = DefaultBrightness;
        private CancellationTokenSource m_cancelToken;
        private Task<Task> m_dimmingTask;

        public Screen()
        {
            m_cancelToken = new CancellationTokenSource(new TimeSpan(0, 0, TimeoutSeconds));
            InitializeAsync();
        }

        public double Brightness
        {
            set
            {
                currentBrightness = value;
                SetBrightness(value);
            }
        }

        public void Dimming(bool down)
        {
            SetBrightness(down ? 0.0 : 100.0);
            //double value = currentBrightness;
            //double target = down ? 0.0 : DefaultBrightness;
            //double increment = (target - value) / (3000 / DimmingCycleTimeInMS);
            //m_dimmingTask = Task.Factory.StartNew(async () =>
            //{
            //    while (Math.Abs(target - value) > Math.Abs(increment))
            //    {
            //        value += increment;
            //        SetBrightness(value);
            //        if (Math.Abs(target - value) < increment)
            //        {
            //            value = target;
            //        }

            //        await Task.Delay(DimmingCycleTimeInMS);
            //    }
            //},
            //m_cancelToken.Token);
        }

        private async void InitializeAsync()
        {
            try
            {
                var i2cDeviceSelector = I2cDevice.GetDeviceSelector();
                var settings = new I2cConnectionSettings(0x45);
                IReadOnlyList<DeviceInformation> devices = await DeviceInformation.FindAllAsync(i2cDeviceSelector);
                if (devices.Count > 0)
                {
                    var deviceId = devices[0].Id;
                    var screen = await I2cDevice.FromIdAsync(deviceId, settings);
                    m_i2CDevice = screen;
                    Brightness = DefaultBrightness;
                }
            }
            catch (Exception)
            { }
        }

        private void SetBrightness(double brightnessInPercent)
        {
            double normalizedBrighnessInPercent = 100.0;
            var max = Math.Min(100.0, brightnessInPercent);
            normalizedBrighnessInPercent = Math.Max(0.0, max);
            //byte brightness = (byte)Math.Round(+0.00007 * Math.Pow(normalizedBrighnessInPercent, 3) 
            //                                    +0.118 * Math.Pow(normalizedBrighnessInPercent, 2) 
            //                                    -6.45 * normalizedBrighnessInPercent);
            byte brightness = (byte)(normalizedBrighnessInPercent / 100.0 * 255.0);
            byte[] writeBuff = new byte[] { 0x86, brightness };
            Task.Factory.StartNew(() =>
            {
                try
                {
                    m_i2CDevice.Write(writeBuff); // Hier wird die Helligkeit effektiv geändert.
                }
                catch (Exception)
                {
                }
            },
            m_cancelToken.Token);
        }
    }
}
