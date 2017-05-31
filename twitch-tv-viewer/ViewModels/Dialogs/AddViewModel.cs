using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using twitch_tv_viewer.Enums;
using twitch_tv_viewer.Repositories.Interfaces;

namespace twitch_tv_viewer.ViewModels.Dialogs
{
    public sealed class AddViewModel : ViewModelBase
    {
        private readonly ISettingsRepository _settingsRepository;

        private string _name;

        public AddViewModel(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
            _name = "";
            CancelCommand = new RelayCommand(Cancel);
            ConfirmCommand = new RelayCommand(Confirm);
        }

        // 

        public string Name
        {
            get => _name;
            set => Set(() => Name, ref _name, value);
        }

        public Action Close { private get; set; }

        // 

        public ICommand CancelCommand { get; }

        public ICommand ConfirmCommand { get; }

        // 

        private void Cancel() => Close();

        private void Confirm() => Add();

        private void Add()
        {
            var name = Name.ToLower().Trim();
            if (name.Length > 0 && !_settingsRepository.Usernames.Contains(name))
            {
                _settingsRepository.Usernames.Add(name);
                _settingsRepository.Save();
                Messenger.Default.Send((ViewAction.Add, name));
                Messenger.Default.Send((MessageType.Notification, "Added channel."));
            }
            Close();
        }
    }
}