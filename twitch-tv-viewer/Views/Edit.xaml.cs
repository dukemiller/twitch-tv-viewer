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
            var charBefore = str.Substring(str.Length - 2, 1);
            var lastCharacter = str.Substring(str.Length-1, 1);
            var isComma = lastCharacter.Equals(",");
            if (e.Key == Key.Space && !isComma || e.Key == Key.OemComma && (isComma || charBefore.Equals(",")))
                e.Handled = true;
            base.OnPreviewKeyDown(e);
        }
    }
}