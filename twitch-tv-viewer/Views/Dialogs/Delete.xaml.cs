using System.Windows;
using twitch_tv_viewer.Models;
using twitch_tv_viewer.ViewModels.Dialogs;

namespace twitch_tv_viewer.Views.Dialogs
{
    /// <summary>
    ///     Interaction logic for Delete.xaml
    /// </summary>
    public partial class Delete
    {
        public Delete()
        {
            InitializeComponent();
        }

        public TwitchChannel Channel { get; set; }

        private void Delete_OnLoaded(object sender, RoutedEventArgs e)
        {
            ((DeleteViewModel) DataContext).Close = Close;
            ((DeleteViewModel) DataContext).Channel = Channel;
        }
    }
}