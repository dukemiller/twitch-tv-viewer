using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using twitch_tv_viewer.Models;
using twitch_tv_viewer.Repositories;
using twitch_tv_viewer.Services;
using twitch_tv_viewer.Views;

namespace twitch_tv_viewer.ViewModels
{
    internal sealed class MainWindowViewModel : ViewModelBase
    {
        private readonly TwitchChannelRepository _twitch;
        private ObservableCollection<TwitchChannel> _channels;
        private readonly TwitchChannelService _twitchService;
        private string _notification;
        private TwitchChannel _selectedChannel;
        private int _counter;

        public MainWindowViewModel()
        {
            Notification = "Loading ...";
            _twitch = new TwitchChannelRepository(new UsernameRepository());
            _twitchService = new TwitchChannelService();

            WindowLoaded = new RelayCommand(OnLoaded);
            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit);
            DeleteCommand = new RelayCommand(Delete);
        }

        public ObservableCollection<TwitchChannel> Channels
        {
            get { return _channels; }
            set
            {
                _channels = value;
                RaisePropertyChanged();
            }
        }

        public string Notification
        {
            get { return _notification; }
            set
            {
                _notification = value;
                RaisePropertyChanged();
            }
        }

        public TwitchChannel SelectedChannel
        {
            get { return _selectedChannel; }
            set
            {
                _selectedChannel = value;
                RaisePropertyChanged();
            }
        }

        // 
        
        public RelayCommand WindowLoaded { get; set; }

        public RelayCommand AddCommand { get; set; }

        public RelayCommand EditCommand { get; set; }

        public RelayCommand DeleteCommand { get; set; }

        private async void Main()
        {
            while (true)
            {
                _counter = 0;
                while (_counter++ < 30)
                    await Task.Delay(1000);
                Channels = new ObservableCollection<TwitchChannel>(await _twitch.GetChannels());
            }
        }
        
        // 


        private async void OnLoaded()
        {
            Channels = new ObservableCollection<TwitchChannel>(await _twitch.GetChannels());
            Notification = "";
            Main();
        }

        private static void Add() => new Add().ShowDialog();

        private static void Edit() => new Edit().ShowDialog();

        private void Delete() => new Delete(SelectedChannel).ShowDialog();


    }
}