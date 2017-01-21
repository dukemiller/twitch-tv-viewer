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
            var viewmodel = SimpleIoc.Default.GetInstance<SettingsViewModel>();
            viewmodel.Close = Close;
            DataContext = viewmodel;
        }
    }
}
