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

// Die Elementvorlage "Benutzersteuerelement" wird unter https://go.microsoft.com/fwlink/?LinkId=234236 dokumentiert.

namespace BoseSoundTouchApp.Controls
{
    public sealed partial class PresetsView : UserControl
    {
        public PresetsView()
        {
            this.InitializeComponent();
            this.Preset1.Tapped += new TappedEventHandler((o, e) => 
            {
                (DataContext as ViewModels.PresetSelectionViewModel).Select(1);
            });
            this.Preset2.Tapped += new TappedEventHandler((o, e) => 
            {
                (DataContext as ViewModels.PresetSelectionViewModel).Select(2);
            });
            this.Preset3.Tapped += new TappedEventHandler((o, e) => 
            {
                (DataContext as ViewModels.PresetSelectionViewModel).Select(3);
            });
            this.Preset4.Tapped += new TappedEventHandler((o, e) => 
            {
                (DataContext as ViewModels.PresetSelectionViewModel).Select(4);
            });
            this.Preset5.Tapped += new TappedEventHandler((o, e) => 
            {
                (DataContext as ViewModels.PresetSelectionViewModel).Select(5);
            });
            this.Preset6.Tapped += new TappedEventHandler((o, e) => 
            {
                (DataContext as ViewModels.PresetSelectionViewModel).Select(6);
            });
        }
    }
}
