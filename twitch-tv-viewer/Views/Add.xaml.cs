using System.Windows;
using twitch_tv_viewer.ViewModels;

namespace twitch_tv_viewer.Views
{
    /// <summary>
    ///     Interaction logic for Add.xaml
    /// </summary>
    public partial class Add : Window
    {
        public Add()
        {
            DataContext = new AddViewModel {Close = Close};
            InitializeComponent();
        }
    }
}