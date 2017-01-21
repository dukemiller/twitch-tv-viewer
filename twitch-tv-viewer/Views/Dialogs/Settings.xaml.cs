using System.Windows;
using twitch_tv_viewer.ViewModels;
using twitch_tv_viewer.ViewModels.Dialogs;

namespace twitch_tv_viewer.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            DataContext = new SettingsViewModel { Close = Close };
        }
    }
}
