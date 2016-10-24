using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using twitch_tv_viewer.Models;

namespace twitch_tv_viewer.ViewModels
{
    internal sealed class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TwitchChannel> _items;
        private string _notification;

        public MainViewModel()
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

            // AddCommand = new RoutedCommand();
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

        // public ICommand AddCommand => new Add().ShowDialog();

        // 

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}