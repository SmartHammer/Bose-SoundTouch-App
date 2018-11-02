using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace BoseSoundTouchApp.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class InfoPage : Page
    {
        public InfoPage()
        {
            InitializeComponent();
            var vm = DataContext as BoseSoundTouchApp.ViewModels.InfoPageViewModel;
            UpdateFoundDevices(vm.FoundDevices);
            vm.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(async (o, e) =>
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    switch (e.PropertyName)
                    {
                        case "FoundDevices":
                            UpdateFoundDevices(vm.FoundDevices);
                            break;
                        default:
                            break;
                    }
                });
            });
        }

        private void UpdateFoundDevices(List<ViewModels.DeviceType> foundDevices)
        {
            this.FoundDevices.Children.Clear();
            double x = 20.0;
            lock (foundDevices)
            {
                foreach (var dev in foundDevices)
                {
                    var icon = new Polygon();
                    icon.Points.Add(new Point(0, 0));
                    icon.Points.Add(new Point(0, 10));
                    icon.Points.Add(new Point(10, 10));
                    icon.Points.Add(new Point(10, 0));
                    icon.Fill = new SolidColorBrush(dev == ViewModels.DeviceType.BoseSoundTouchDevice ? 
                                                    Windows.UI.Colors.DarkBlue : 
                                                    //dev == ViewModels.DeviceType.MediaServer ? 
                                                    //Windows.UI.Colors.Silver : 
                                                    Windows.UI.Colors.Silver);
                    Canvas.SetLeft(icon, x);
                    Canvas.SetTop(icon, 45);
                    this.FoundDevices.Children.Add(icon);
                    x += 15.0;
                }
            }
        }
    }
}
