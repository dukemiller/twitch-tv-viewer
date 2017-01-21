using System.Windows;
using twitch_tv_viewer.Models;
using twitch_tv_viewer.ViewModels;
using twitch_tv_viewer.ViewModels.Dialogs;

namespace twitch_tv_viewer.Views.Dialogs
{
    /// <summary>
    ///     Interaction logic for Delete.xaml
    /// </summary>
    public partial class Delete : Window
    {
        public Delete(TwitchChannel channel)
        {
            DataContext = new DeleteViewModel {Close = Close, Channel = channel};
            InitializeComponent();
        }
    }
}