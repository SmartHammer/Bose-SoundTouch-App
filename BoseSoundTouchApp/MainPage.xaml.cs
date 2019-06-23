using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x407 dokumentiert.

namespace BoseSoundTouchApp
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Models.IGeneralModel m_model;
        private DispatcherTimer m_timer;

        public MainPage()
        {
            InitializeComponent();
            InitializeModel();
            InitializeEvents();
            InitializePage();
            Start();
        }

        private void InitializeModel()
        {
            var searcher = new Models.DeviceSearcher();
            var screen = new Models.Screen();
            m_timer = new DispatcherTimer();
            m_timer.Tick += new EventHandler<object>((obj, evt) =>
            {
                m_model.Start();
            });
            m_timer.Interval = new TimeSpan(0, 0, 30);
            m_model = new Models.GeneralModel(searcher, screen);
        }

        private void InitializeEvents()
        {
            m_model.ModelInitializationFinished += new Models.ModelInitialized.Handler(async (o, e) =>
            {
                await MainFrame.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    if (e.Success)
                    {
                        m_timer.Stop();
                        MainFrame.Navigate(typeof(Views.RunningPage));
                        ((MainFrame.Content as Views.RunningPage).DataContext as ViewModels.RunningPageViewModel).Model = m_model;
                        ((MainFrame.Content as Views.RunningPage).DataContext as ViewModels.RunningPageViewModel).Dispatcher = Dispatcher;
                    }
                    else
                    {
                        m_timer.Start();
                    }
                });
             });

            this.Unloaded += new RoutedEventHandler((o, e) =>
            {
                m_model.Stop();
            });
        }

        private void InitializePage()
        {
            MainFrame.Navigate(typeof(Views.InfoPage));
            ((MainFrame.Content as Views.InfoPage).DataContext as ViewModels.InfoPageViewModel).Model = m_model;
        }

        private void Start()
        {
            m_model.Start();
        }
    }
}
