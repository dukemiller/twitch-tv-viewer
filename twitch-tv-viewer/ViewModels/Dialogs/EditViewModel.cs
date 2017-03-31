using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using twitch_tv_viewer.Enums;
using twitch_tv_viewer.Repositories;

namespace twitch_tv_viewer.ViewModels.Dialogs
{
    internal class EditViewModel : ViewModelBase
    {
        private readonly IUsernameRepository _user;

        private string _usernames;

        public EditViewModel()
        {
            _user = SimpleIoc.Default.GetInstance<IUsernameRepository>();
            Usernames = string.Join(", ", _user.GetUsernames());
            ConfirmCommand = new RelayCommand(Confirm);
            CancelCommand = new RelayCommand(Cancel);
        }

        public ICommand ConfirmCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public Action Close { private get; set; }

        private void Confirm()
        {
            var usernames = Regex
                .Split(Regex.Replace(Usernames.TrimEnd(','), @"\s+", @""), ",")
                .Where(s => s.Length > 0);
            _user.SetUsernames(usernames);
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
