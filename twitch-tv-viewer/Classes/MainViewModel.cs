using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace twitch_tv_viewer.Classes
{
    internal sealed class MainViewModel: INotifyPropertyChanged
    {
        private ObservableCollection<string> _items;
        private string _notification;

        public MainViewModel()
        {
            Items = new ObservableCollection<string> {"A", "B", "C"};
            Notification = "Notification text";
        }

        public ObservableCollection<string> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged();
            }
        }

        public string Notification
        {
            get { return _notification; }
            set
            {
                _notification = value;
                OnPropertyChanged();
            }
        }

        // 

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
