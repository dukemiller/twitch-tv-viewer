using System.Windows;
using GalaSoft.MvvmLight.Ioc;
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
        }

        private void Settings_OnLoaded(object sender, RoutedEventArgs e)
        {
            ((SettingsViewModel)DataContext).Close = Close;
        }
    }
}
