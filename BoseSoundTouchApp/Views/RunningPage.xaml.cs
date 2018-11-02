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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.UI.Animations;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace BoseSoundTouchApp.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class RunningPage : Page
    {
        private DispatcherTimer m_timer;

        public RunningPage()
        {
            this.InitializeComponent();
            this.m_timer = new DispatcherTimer();
            //BackgroundIcon.Blur(duration:0, delay:0, value:100).StartAsync();
            PresetSelection.Tapped += new TappedEventHandler((o, e) =>
            {
                (DataContext as ViewModels.RunningPageViewModel).StartPresetSelectionView();
                (this.Resources["showPresetPanel"] as Storyboard).Begin();
                m_timer.Interval = AutoDeactivateTime;
                m_timer.Tick += new EventHandler<object>((obj, evt) =>
                {
                    (DataContext as ViewModels.RunningPageViewModel).StartRunningView();
                    (this.Resources["hidePresetPanel"] as Storyboard).Begin();
                });
                m_timer.Start();
            });
            DeviceSelection.Tapped += new TappedEventHandler((o, e) =>
            {
                (DataContext as ViewModels.RunningPageViewModel).StartDeviceSelectionView();
                (this.Resources["showDeviceSelectionPanel"] as Storyboard).Begin();
                m_timer.Interval = AutoDeactivateTime;
                m_timer.Tick += new EventHandler<object>((obj, evt) =>
                {
                    (DataContext as ViewModels.RunningPageViewModel).StartRunningView();
                    (this.Resources["hideDeviceSelectionPanel"] as Storyboard).Begin();
                });
                m_timer.Start();
            });
            PresetDeselection.Tapped += new TappedEventHandler((o, e) =>
            {
                (DataContext as ViewModels.RunningPageViewModel).StartRunningView();
                (this.Resources["hidePresetPanel"] as Storyboard).Begin();
                m_timer.Stop();
            });
            DeviceDeselection.Tapped += new TappedEventHandler((o, e) =>
            {
                (DataContext as ViewModels.RunningPageViewModel).StartRunningView();
                (this.Resources["hideDeviceSelectionPanel"] as Storyboard).Begin();
                m_timer.Stop();
            });
            PreviousDevice.Tapped += new TappedEventHandler((o, e) =>
            {
                (DataContext as ViewModels.RunningPageViewModel).PreviousDevice();
                m_timer.Stop();
                m_timer.Interval = AutoDeactivateTime;
                m_timer.Start();
            });
            NextDevice.Tapped += new TappedEventHandler((o, e) =>
            {
                (DataContext as ViewModels.RunningPageViewModel).NextDevice();
                m_timer.Stop();
                m_timer.Interval = AutoDeactivateTime;
                m_timer.Start();
            });
            Loaded += new RoutedEventHandler((o, e) =>
            {
                (DataContext as ViewModels.RunningPageViewModel).Initialize();
            });
            MainPage.Tapped += new TappedEventHandler((o, e) =>
            {
                (DataContext as ViewModels.RunningPageViewModel).UpDimming();
            });
            VolumeDown.Tapped += new TappedEventHandler((o, e) =>
            {
                (DataContext as ViewModels.RunningPageViewModel).VolumeDown();
            });
            VolumeUp.Tapped += new TappedEventHandler((o, e) =>
            {
                (DataContext as ViewModels.RunningPageViewModel).VolumeUp();
            });
        }

        private TimeSpan AutoDeactivateTime => TimeSpan.FromSeconds(10.0);
    }
}
