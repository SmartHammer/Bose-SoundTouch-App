// Die Elementvorlage "Benutzersteuerelement" wird unter https://go.microsoft.com/fwlink/?LinkId=234236 dokumentiert.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace BoseSoundTouchApp.Controls
{
    public sealed partial class InProgress : UserControl
    {
        private Windows.UI.Color m_color = Windows.UI.Colors.White;
        public InProgress()
        {
            InitializeComponent();
            Loaded += new Windows.UI.Xaml.RoutedEventHandler((o, t) => 
            {
                spinner.Begin();
                foreach (var child in spinner_1.Children)
                {
                    (child as Polygon).Fill = new Windows.UI.Xaml.Media.SolidColorBrush(m_color);
                }
            });
        }

        public Windows.UI.Color Color
        {
            get
            {
                return m_color;
            }

            set
            {
                m_color = value;
                foreach (var child in spinner_1.Children)
                {
                    (child as Polygon).Fill = new Windows.UI.Xaml.Media.SolidColorBrush(m_color);
                }
            }
        }
    }
}
