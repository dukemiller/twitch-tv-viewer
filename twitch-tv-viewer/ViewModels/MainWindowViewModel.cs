﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight.CommandWpf;
using twitch_tv_viewer.Models;
using twitch_tv_viewer.Repositories;
using twitch_tv_viewer.Views;

namespace twitch_tv_viewer.ViewModels
{
    internal sealed class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly TwitchChannelRepository _twitch;
        private ObservableCollection<TwitchChannel> _channels;
        private string _notification;
        private TwitchChannel _selectedChannel;

        public MainWindowViewModel()
        {
            Notification = "Loading ...";
            _twitch = new TwitchChannelRepository(new UsernameRepository());

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

        public RelayCommand WindowLoaded { get; set; }

        public RelayCommand AddCommand { get; set; }

        public RelayCommand EditCommand { get; set; }

        public RelayCommand DeleteCommand { get; set; }

        // 

        public event PropertyChangedEventHandler PropertyChanged;

        private async void OnLoaded()
        {
            Channels = new ObservableCollection<TwitchChannel>(await _twitch.GetChannels());
            Notification = "";
        }

        private static void Add()
        {
            new Add().ShowDialog();
        }

        private static void Edit()
        {
            new Edit().ShowDialog();
        }

        private void Delete()
        {
            new Delete(SelectedChannel).ShowDialog();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}