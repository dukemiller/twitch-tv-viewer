using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using twitch_tv_viewer.ViewModels;

namespace twitch_tv_viewer.Views
{
    /// <summary>
    ///     Interaction logic for Edit.xaml
    /// </summary>
    public partial class Edit : Window
    {
        private readonly EditViewModel _editViewModel;
        public Edit()
        {
            _editViewModel = new EditViewModel { Close = Close };
            DataContext = _editViewModel;
            InitializeComponent();
        }

        private static readonly Regex Regex = new Regex("^[a-zA-Z_0-9,]+$");

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
        
        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var str = _editViewModel.Usernames;

            if (str.Length == 0 && (e.Key == Key.OemComma || e.Key == Key.Space))
                e.Handled = true;

            else if (str.Length >= 1)
            {
                var previousIsComma = str.Skip(str.Length - 1).First().Equals(',');
                var previousIsSpace = str.Skip(str.Length - 1).First().Equals(' ');

                if (!previousIsComma && e.Key == Key.Space  ||
                    (previousIsSpace || previousIsComma) && e.Key == Key.OemComma)
                    e.Handled = true;
            }

            base.OnPreviewKeyDown(e);
        }
    }
}