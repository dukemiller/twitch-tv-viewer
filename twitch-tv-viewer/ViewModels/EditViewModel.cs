using System;
using System.Linq;
using System.Text.RegularExpressions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using twitch_tv_viewer.Repositories;

namespace twitch_tv_viewer.ViewModels
{
    internal class EditViewModel : ViewModelBase
    {
        private readonly UsernameRepository _user;
        private string _usernames;

        public EditViewModel()
        {
            _user = new UsernameRepository();
            Usernames = string.Join(", ", _user.GetUsernames());
            ConfirmCommand = new RelayCommand(Confirm);
            CancelCommand = new RelayCommand(Cancel);
        }

        public RelayCommand ConfirmCommand { get; set; }

        public RelayCommand CancelCommand { get; set; }

        public Action Close { private get; set; }

        private void Confirm()
        {
            var usernames = Regex
                .Split(Regex.Replace(Usernames, @"\s+", @" "), ", ")
                .Where(s => s.Length > 0);
            _user.SetUsernames(usernames);
            MessengerInstance.Send(new ResetMessage());
            Close();
        }

        private void Cancel() => Close();

        public string Usernames
        {
            get { return _usernames; }
            set
            {
                _usernames = value; 
                RaisePropertyChanged();
            }
        }
    }
}
