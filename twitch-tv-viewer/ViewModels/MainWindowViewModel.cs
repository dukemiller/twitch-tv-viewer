using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using twitch_tv_viewer.Models;
using twitch_tv_viewer.Views;

namespace twitch_tv_viewer.ViewModels
{
    internal sealed class MainWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TwitchChannel> _items;
        private string _notification;
        private TwitchChannel _selectedChannel;

        public MainWindowViewModel()
        {
            Items = new ObservableCollection<TwitchChannel>
            {
                new TwitchChannel
                {
                    Game = "Dota 2",
                    Name = "Arteezy",
                    Status = "Playing games",
                    Viewers = "19248"
                },

                new TwitchChannel
                {
                    Game = "League of Legends",
                    Name = "Destiny",
                    Status = "Watch me dive 1v4 and rage",
                    Viewers = "200"
                },

                new TwitchChannel
                {
                    Game = "Banjo-Kazooie",
                    Name = "Stivitybobo",
                    Status = "I dont like this game anymore",
                    Viewers = "852"
                }
            };

            Notification = "Notification text";

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit);
            DeleteCommand = new RelayCommand(Delete);

        }

        public ObservableCollection<TwitchChannel> Items
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
        
        public TwitchChannel SelectedChannel
        {
            get { return _selectedChannel; }
            set
            {
                _selectedChannel = value;
                OnPropertyChanged();
            }
        }

        // 

        public RelayCommand AddCommand { get; set; }

        public RelayCommand EditCommand { get; set; }

        public RelayCommand DeleteCommand { get; set; }

        private static void Add()
        {
            new Add().ShowDialog();
        }

        private static void Edit()
        {
            new Edit().ShowDialog();
        }

        private static void Delete()
        {
            new Delete().ShowDialog();
        }

        // 

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}