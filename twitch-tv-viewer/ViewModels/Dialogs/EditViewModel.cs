using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using twitch_tv_viewer.Enums;
using twitch_tv_viewer.Repositories.Interfaces;

namespace twitch_tv_viewer.ViewModels.Dialogs
{
    public class EditViewModel : ViewModelBase
    {
        private readonly ISettingsRepository _settingsRepository;

        private string _usernames;

        // 

        public EditViewModel(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
            Usernames = string.Join(", ", _settingsRepository.Usernames);
            ConfirmCommand = new RelayCommand(Confirm);
            CancelCommand = new RelayCommand(Cancel);
        }

        // 

        public ICommand ConfirmCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public Action Close { private get; set; }

        private void Confirm()
        {
            var usernames = Regex
                .Split(Regex.Replace(Usernames.TrimEnd(','), @"\s+", @""), ",")
                .Where(s => s.Length > 0)
                .Select(username => username.ToLower().Trim());
            var collection = new ObservableCollection<string>();
            foreach(var username in usernames)
                if (!collection.Contains(username))
                    collection.Add(username);
            _settingsRepository.Usernames = collection;
            _settingsRepository.Save();
            MessengerInstance.Send(ViewAction.Reset);
            Close();
        }

        private void Cancel() => Close();

        public string Usernames
        {
            get => _usernames;
            set => Set(() => Usernames, ref _usernames, value);
        }
    }
}
