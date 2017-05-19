using System.Windows.Controls;
using System.Windows.Input;

namespace twitch_tv_viewer.Views.Components
{
    /// <summary>
    /// Interaction logic for StreamBox.xaml
    /// </summary>
    public partial class ChannelsDisplay : UserControl
    {
        public ChannelsDisplay()
        {
            InitializeComponent();
        }

        private void UIElement_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is ScrollViewer)
            {
                ((DataGrid)sender).UnselectAll();
            }
        }
    }
}
