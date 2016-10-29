using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using twitch_tv_viewer.Models;
using twitch_tv_viewer.Repositories;
using twitch_tv_viewer.Services;
using twitch_tv_viewer.Views;

namespace twitch_tv_viewer.ViewModels.Components
{
    internal class ChannelsDisplayViewModel : ViewModelBase
    {
        private readonly TwitchChannelService _twitchService;

        private readonly TwitchChannelRepository _twitch;

        private readonly UsernameRepository _user;

        private ObservableCollection<TwitchChannel> _channels;

        public int Counter { get; private set; }

        private TwitchChannel _selectedChannel;

        public ChannelsDisplayViewModel()
        {
            _user = new UsernameRepository();
            _twitch = new TwitchChannelRepository(_user);
            _twitchService = new TwitchChannelService();

            DeleteCommand = new RelayCommand(Delete);
            ClickCommand = new RelayCommand(Click);
            OpenChatCommand = new RelayCommand(OpenChat);
            WindowLoaded = new RelayCommand(OnLoaded);
            CopyCommand = new RelayCommand(Copy);
            AddCommand = new RelayCommand(Add);

            // on any sent message, set the counter to 30 to instantly refresh
            Messenger.Default.Register<ResetMessage>(this, message => Counter = 30);
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

        public TwitchChannel SelectedChannel
        {
            get { return _selectedChannel; }
            set
            {
                _selectedChannel = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand WindowLoaded { get; set; }

        public RelayCommand DeleteCommand { get; set; }

        public RelayCommand ClickCommand { get; set; }

        public RelayCommand OpenChatCommand { get; set; }

        public RelayCommand CopyCommand { get; set; }

        public RelayCommand AddCommand { get; set; }

        private async void OnLoaded() => await Main();

        private void Delete() => new Delete(SelectedChannel).ShowDialog();

        private async void Click()
        {
            if (SelectedChannel != null)
            {
                Messenger.Default.Send(new NotificationMessage
                {
                    Message = $"Opening stream for {SelectedChannel.Name} ..."
                });
                await _twitchService.PlayVideo(SelectedChannel);
            }
        }

        private void Copy()
        {
            if (SelectedChannel != null)
            {
                Clipboard.Clear();
                Clipboard.SetText(SelectedChannel.Name);
            }
        }

        private static void Add() => new Add().ShowDialog();

        private async Task Main()
        {
            while (true)
            {
                if (!_user.GetUsernames().Any())
                    Messenger.Default.Send(new Result {Message = "Add some twitch usernames."});

                else
                {
                    var result = await _twitch.GetChannels();

                    MessengerInstance.Send(new NotificationMessage());

                    if (result.Any())
                    {
                        Channels = new ObservableCollection<TwitchChannel>(result);
                        Messenger.Default.Send(new Result {Successful = true});
                    }

                    else
                        Messenger.Default.Send(new Result {Message = "No streamers online."});
                }

                Counter = 0;
                while (Counter++ < 30)
                    await Task.Delay(1000);
            }
        }

        private void OpenChat()
        {
            if (SelectedChannel != null)
            {
                Messenger.Default.Send(new NotificationMessage
                {
                    Message = $"Opening chat for {SelectedChannel.Name} ..."
                });
                _twitchService.OpenChat(SelectedChannel);
            }
        }
    }
}