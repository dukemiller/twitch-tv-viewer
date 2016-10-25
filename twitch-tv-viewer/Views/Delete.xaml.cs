using System.Windows;
using twitch_tv_viewer.Models;
using twitch_tv_viewer.ViewModels;

namespace twitch_tv_viewer.Views
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