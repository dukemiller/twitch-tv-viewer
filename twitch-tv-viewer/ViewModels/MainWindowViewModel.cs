﻿using System;
using System.IO;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using twitch_tv_viewer.Enums;
using twitch_tv_viewer.Repositories.Interfaces;
using twitch_tv_viewer.ViewModels.Components;
using twitch_tv_viewer.Views.Dialogs;

namespace twitch_tv_viewer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public static string ApplicationPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "twitch_tv_viewer");

        private readonly MessageDisplayViewModel _messageDisplay;

        private readonly ChannelsDisplayViewModel _channelsDisplay;

        private ViewModelBase _currentViewModel;

        private string _notification;

        private readonly ISettingsRepository _settings;

        // 

        public MainWindowViewModel(ISettingsRepository settings,
            MessageDisplayViewModel messageDisplay,
            ChannelsDisplayViewModel channelDisplay)
        {
            _settings = settings;
            _messageDisplay = messageDisplay;
            _channelsDisplay = channelDisplay;

            Notification = "Loading ...";
            CurrentViewModel = _channelsDisplay;

            MessengerInstance.Register<(bool, string)>(this, DisplayLogic);
            MessengerInstance.Register<(MessageType Type, string Message)>(this, ChangeNotification);

            SettingsCommand = new RelayCommand(OpenSettings);
            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit);
            RefreshCommand = new RelayCommand(Refresh);
            SortCommand = new RelayCommand(Sort);
        }

        /// <summary>
        ///     A more explicit way to change the notification.
        /// </summary>
        private void ChangeNotification((MessageType Type, string Content) message)
        {
            // A quirk with the notification auto fade-out, it'll only fade if the message
            // is different in .Equals() to the previous
            if (message.Type == MessageType.Notification)
            {
                Notification = "";
                Notification = message.Content;
            }
        }

        // 

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                CurrentViewModel?.Cleanup();
                Set(() => CurrentViewModel, ref _currentViewModel, value);
            }
        }

        public string Notification
        {
            get => _notification;
            set => Set(() => Notification, ref _notification, value);
        }

        private void DisplayLogic((bool successful, string message) result)
        {
            if (result.successful)
                CurrentViewModel = _channelsDisplay;
            else
            {
                CurrentViewModel = _messageDisplay;
                _messageDisplay.Message = result.message;
            }
        }

        public string SortName => _settings.SortName;

        // 

        public RelayCommand SettingsCommand { get; set; }

        public RelayCommand AddCommand { get; set; }

        public RelayCommand EditCommand { get; set; }

        public RelayCommand RefreshCommand { get; set; }

        public RelayCommand SortCommand { get; set; }

        // 

        private static void OpenSettings() => new Settings().ShowDialog();

        private async void Refresh()
        {
            Notification = "";
            Notification = "Refreshing ...";
            await Task.Delay(500);
            MessengerInstance.Send(ViewAction.Reset);
        }

        private void Sort()
        {
            _settings.SortBy = (_settings.SortBy + 1) % 4;
            _channelsDisplay.Sort(_settings.SortName);
            _settings.Save();
            RaisePropertyChanged(nameof(_settings.SortName));
            Messenger.Default.Send((MessageType.Notification, $"Changed sort to {_settings.SortName}."));
        }

        private static void Add() => new Add().ShowDialog();

        private static void Edit() => new Edit().ShowDialog();
    }
}