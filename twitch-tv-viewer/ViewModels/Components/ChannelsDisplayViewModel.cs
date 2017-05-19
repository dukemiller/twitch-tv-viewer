using System;
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
using twitch_tv_viewer.Enums;
using twitch_tv_viewer.Models;
using twitch_tv_viewer.Repositories.Interfaces;
using twitch_tv_viewer.Services.Interfaces;
using Add = twitch_tv_viewer.Views.Dialogs.Add;
using Delete = twitch_tv_viewer.Views.Dialogs.Delete;

namespace twitch_tv_viewer.ViewModels.Components
{
    public class ChannelsDisplayViewModel : ViewModelBase
    {
        private readonly ISettingsRepository _settings;

        private readonly ISoundPlayerService _soundPlayer;

        private readonly ITwitchChannelService _twitchService;

        private int _lastCount = -1;

        private ObservableCollection<TwitchChannel> _channels;

        private TwitchChannel _selectedChannel;

        public static int CounterMax = 30000;

        public static int CounterInterval = 100;

        // 

        public ChannelsDisplayViewModel(ISettingsRepository settings, ISoundPlayerService sound, ITwitchChannelService twitchSevice)
        {
            _settings = settings;
            _soundPlayer = sound;
            _twitchService = twitchSevice;
            _channels = new ObservableCollection<TwitchChannel>();

            DeleteCommand = new RelayCommand(Delete);
            ClickCommand = new RelayCommand(Click);
            OpenChatCommand = new RelayCommand(OpenChat);
            OpenStreamCommand = new RelayCommand(OpenStream);
            WindowLoaded = new RelayCommand(OnLoaded);
            CopyCommand = new RelayCommand(Copy);
            AddCommand = new RelayCommand(Add);
            PromoteCommand = new RelayCommand(Promote);

            // on reset, set the counter to 30 to refresh
            Messenger.Default.Register<ViewAction>(this, message =>
            {
                if (message == ViewAction.Reset)
                    Counter = CounterMax;
            });
        }

        // 

        public int Counter { get; private set; }

        public ObservableCollection<TwitchChannel> Channels
        {
            get => _channels;
            set => Set(() => Channels, ref _channels, value);
        }

        public TwitchChannel SelectedChannel
        {
            get => _selectedChannel;
            set => Set(() => SelectedChannel, ref _selectedChannel, value);
        }

        public ICommand WindowLoaded { get; set; }

        public ICommand DeleteCommand { get; set; }

        public ICommand ClickCommand { get; set; }

        public ICommand OpenChatCommand { get; set; }

        public ICommand OpenStreamCommand { get; set; }

        public ICommand CopyCommand { get; set; }

        public ICommand AddCommand { get; set; }

        public ICommand PromoteCommand { get; set; }

        //


        private async Task Main()
        {
            while (true)
            {
                if (!_settings.Usernames.Any())
                    Messenger.Default.Send((false, "Add some twitch usernames."));

                else
                    try
                    {
                        var result = await _twitchService.GetChannels();

                        // success
                        if (result.Any())
                        {
                            foreach (var channel in result)
                                channel.Promoted = _settings.Important.Contains(channel.Name.ToLower());

                            var newChannels = result.Where(r2 => !Channels.Any(ch => ch.Name.Equals(r2.Name))).ToList();
                            var goneChannels = Channels.Where(c2 => !result.Any(c1 => c1.Name.Equals(c2.Name))).ToList();
                            var updatedChannels = result.Where(r2 => Channels.Any(ch => ch.Name.Equals(r2.Name)));

                            foreach (var channel in goneChannels)
                                Channels.Remove(channel);

                            foreach (var channel in newChannels)
                                Channels.Add(channel);

                            foreach (var channel in updatedChannels)
                            {
                                var already = Channels.First(ch => ch.Name.Equals(channel.Name));
                                already.Viewers = channel.Viewers;
                                already.Game = channel.Game;
                                already.Status = channel.Status;
                            }

                            if (newChannels.Any() || goneChannels.Any())
                            {
                                Sort(_settings.SortName);
                                MessengerInstance.Send((true, ""));
                                MessengerInstance.Send((MessageType.Notification, $"{Channels.Count} streams available."));
                            }
                        }

                        // success but no streamers
                        else
                            MessengerInstance.Send((false, "No streamers online."));

                        // play sound
                        if (Channels.Count != _lastCount && _lastCount != -1 && _settings.UserAlert)
                            if (_lastCount > Channels.Count)
                                _soundPlayer.PlayOfflineSound();
                            else if (_lastCount < Channels.Count)
                                _soundPlayer.PlayOnlineSound();

                        _lastCount = Channels.Count;
                    }

                    catch (Exception e)
                    {
                        MessengerInstance.Send((false, $"Connectivity issue.\n{e}"));
                    }

                Counter = 0;
                while (Counter < CounterMax)
                {
                    Counter += CounterInterval;
                    await Task.Delay(CounterInterval);
                }
            }
        }

        private void Promote()
        {
            if (_settings.Important.Contains(SelectedChannel.Name.ToLower()))
            {
                _settings.Important.Remove(SelectedChannel.Name.ToLower());
                Channels.First(ch => ch.Name.Equals(SelectedChannel.Name)).Promoted = false;
                Messenger.Default.Send((MessageType.Notification, $"Demoted {SelectedChannel.Name}."));
            }
            else
            {
                _settings.Important.Add(SelectedChannel.Name.ToLower());
                Channels.First(ch => ch.Name.Equals(SelectedChannel.Name)).Promoted = true;
                Messenger.Default.Send((MessageType.Notification, $"Promoted {SelectedChannel.Name}."));
            }
            _settings.Save();
        }

        public void Sort(string propertyName)
        {
            var propertyDescriptor = TypeDescriptor
                .GetProperties(typeof(TwitchChannel))
                .Find(propertyName, true);

            // Ideally this would just be for any numeric type
            if (propertyName.Equals("Viewers"))
                Channels =
                    new ObservableCollection<TwitchChannel>(
                        Channels.OrderByDescending(c => c.Viewers.All(char.IsNumber) ? int.Parse(c.Viewers) : 0));
            else
                Channels =
                    new ObservableCollection<TwitchChannel>(Channels.OrderBy(c => propertyDescriptor.GetValue(c)));

            // 
        }

        private async void OnLoaded() => await Main();

        private void Delete()
        {
            if (SelectedChannel == null)
                return;

            new Delete {Channel = SelectedChannel}.ShowDialog();
        }

        private async void Click()
        {
            if (SelectedChannel == null)
                return;

            Messenger.Default.Send((MessageType.Notification, $"Opening stream for {SelectedChannel.Name} ..."));

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

        private void Copy()
        {
            if (SelectedChannel == null)
                return;

            Clipboard.Clear();
            Clipboard.SetText(SelectedChannel.Name);
            MessengerInstance.Send((MessageType.Notification, "Copied channel to clipboard."));
        }

        private static void Add() => new Add().ShowDialog();

        private void OpenChat()
        {
            if (SelectedChannel == null)
                return;

            MessengerInstance.Send((MessageType.Notification, $"Opening chat for {SelectedChannel.Name} ..."));
            _twitchService.OpenChat(SelectedChannel);
        }

        private void OpenStream()
        {
            if (SelectedChannel == null)
                return;

            MessengerInstance.Send((MessageType.Notification, $"Opening channel page for {SelectedChannel.Name} ..."));
            _twitchService.OpenStream(SelectedChannel);
        }
    }
}