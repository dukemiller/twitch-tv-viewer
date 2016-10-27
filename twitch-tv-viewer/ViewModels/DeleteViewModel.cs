using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using twitch_tv_viewer.Models;
using twitch_tv_viewer.Repositories;

namespace twitch_tv_viewer.ViewModels
{
    internal sealed class DeleteViewModel : ViewModelBase
    {
        private readonly UsernameRepository _usernames;

        public DeleteViewModel()
        {
            _usernames = new UsernameRepository();
            CancelCommand = new RelayCommand(Cancel);
            ConfirmCommand = new RelayCommand(Confirm);
        }

        public Action Close { get; set; }

        public TwitchChannel Channel { get; set; }

        // 

        public RelayCommand CancelCommand { get; private set; }

        public RelayCommand ConfirmCommand { get; private set; }

        private void Cancel() => Close();

        private void Confirm() => Delete();

        private void Delete()
        {
            _usernames.RemoveUsername(Channel.Name);
            Messenger.Default.Send(new ResetMessage());
            Close();
        }
    }
}