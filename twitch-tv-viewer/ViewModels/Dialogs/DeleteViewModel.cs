using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using twitch_tv_viewer.Enums;
using twitch_tv_viewer.Models;
using twitch_tv_viewer.Repositories.Interfaces;

namespace twitch_tv_viewer.ViewModels.Dialogs
{
    public class DeleteViewModel : ViewModelBase
    {
        private readonly ISettingsRepository _settingsRepository;

        private TwitchChannel _channel;

        public DeleteViewModel(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
            CancelCommand = new RelayCommand(Cancel);
            ConfirmCommand = new RelayCommand(Confirm);
        }

        public Action Close { get; set; }

        public TwitchChannel Channel
        {
            get => _channel;
            set { Set(() => Channel, ref _channel, value); }
        }

        // 

        public ICommand CancelCommand { get; }

        public ICommand ConfirmCommand { get; }

        // 

        private void Cancel() => Close();

        private void Confirm() => Delete();

        private void Delete()
        {
            _settingsRepository.Usernames.Remove(Channel.Name.ToLower().Trim());
            _settingsRepository.Save();
            Messenger.Default.Send((ViewAction.Delete, Channel.Name.ToLower()));
            Messenger.Default.Send((MessageType.Notification, "Deleted channel."));
            Close();
        }
    }
}