using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using twitch_tv_viewer.ViewModels.Dialogs;

namespace twitch_tv_viewer.Views.Dialogs
{
    /// <summary>
    ///     Interaction logic for Edit.xaml
    /// </summary>
    public partial class Edit
    {
        public Edit()
        {
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
            var str = ((EditViewModel)DataContext).Usernames;

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

        private void Edit_OnLoaded(object sender, RoutedEventArgs e)
        {
            ((EditViewModel)DataContext).Close = Close;
        }
    }
}