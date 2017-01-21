using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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
        private readonly ITwitchChannelService _twitchService;

        private readonly ITwitchChannelRepository _twitch;

        private readonly ISoundPlayerService _soundPlayer;

        private readonly IUsernameRepository _user;

        private readonly ISettingsRepository _settings;

        private ObservableCollection<TwitchChannel> _channels;
        
        private int _lastCount = -1;

        public int Counter { get; private set; }

        private TwitchChannel _selectedChannel;

        public ChannelsDisplayViewModel()
        {
            _user = new UsernameRepository();
            _settings = new SettingsRepository();
            _twitch = new TwitchChannelRepository(_user);
            _twitchService = new TwitchChannelService();
            _soundPlayer = new SoundPlayerService();

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

        public ICommand WindowLoaded { get; set; }

        public ICommand DeleteCommand { get; set; }

        public ICommand ClickCommand { get; set; }

        public ICommand OpenChatCommand { get; set; }

        public ICommand CopyCommand { get; set; }

        public ICommand AddCommand { get; set; }

        //

        public void Sort(string propertyName)
        {
            var propertyDescriptor = TypeDescriptor
                    .GetProperties(typeof(TwitchChannel))
                    .Find(propertyName, true);

            // Ideally this would just be for any numeric type
            if (propertyName.Equals("Viewers"))
                Channels = new ObservableCollection<TwitchChannel>(Channels.OrderByDescending(c => c.Viewers.All(char.IsNumber) ? int.Parse(c.Viewers) : 0));
            else
                Channels = new ObservableCollection<TwitchChannel>(Channels.OrderBy(c => propertyDescriptor.GetValue(c)));
        }

        private async void OnLoaded() => await Main();

        private void Delete()
        {
            if (SelectedChannel != null)
                new Delete(SelectedChannel).ShowDialog();
        }

        private async void Click()
        {
            if (SelectedChannel != null)
            {
                Messenger.Default.Send(new NotificationMessage
                {
                    Message = $"Opening stream for {SelectedChannel.Name} ..."
                });

                var video = await _twitchService.PlayVideo(SelectedChannel);
                var information = Regex.Split(video, "\n");

                // Handle unique butterfly streams that don't have a "source" quality
                if (information.Any(line => line.Contains("Available streams: ")) 
                    && information.Any(line => line.StartsWith("error: ")))
                {
                    var streams = information.First(line => line.Contains("Available streams: "));
                    var resolutionLine = Regex.Split(streams, ": ")[1];
                    var resolutions = Regex.Split(resolutionLine, ", ").TakeWhile(s => s.Any(char.IsNumber));
                    await _twitchService.PlayVideo(SelectedChannel, resolutions.Last());
                }
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
                    try
                    {
                        var result = await _twitch.GetChannels();

                        MessengerInstance.Send(new NotificationMessage());

                        if (result.Any())
                        {
                            Channels = new ObservableCollection<TwitchChannel>(result);
                            Sort(_settings.SortName);
                            MessengerInstance.Send(new Result {Successful = true});
                        }

                        else
                            MessengerInstance.Send(new Result {Message = "No streamers online."});

                        if (Channels.Count != _lastCount && _lastCount != -1 && _settings.UserAlert)
                        {
                            if (_lastCount > Channels.Count)
                                _soundPlayer.PlayOfflineSound();
                            else if (_lastCount < Channels.Count)
                                _soundPlayer.PlayOnlineSound();
                        }

                        _lastCount = Channels.Count;
                    }

                    catch
                    {
                        MessengerInstance.Send(new Result { Message = "Connectivity issue." });
                    }
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
                MessengerInstance.Send(new NotificationMessage
                {
                    Message = $"Opening chat for {SelectedChannel.Name} ..."
                });
                _twitchService.OpenChat(SelectedChannel);
            }
        }
    }
}