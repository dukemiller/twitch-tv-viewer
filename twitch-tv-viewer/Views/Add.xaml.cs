using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
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

        private static readonly Regex Regex = new Regex("^[a-zA-Z_0-9]+$");

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
            base.OnPreviewKeyDown(e);
        }
    }
}