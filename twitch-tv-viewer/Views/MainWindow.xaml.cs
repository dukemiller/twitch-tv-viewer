using System.Windows;

namespace twitch_tv_viewer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Edit_Click(object sender, RoutedEventArgs e) => new Edit().ShowDialog();

        private void Add_Click(object sender, RoutedEventArgs e) => new Add().ShowDialog();
    }
}
