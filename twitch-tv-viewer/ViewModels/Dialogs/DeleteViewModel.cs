using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using twitch_tv_viewer.Enums;
using twitch_tv_viewer.Models;
using twitch_tv_viewer.Repositories;

namespace twitch_tv_viewer.ViewModels.Dialogs
{
    internal sealed class DeleteViewModel : ViewModelBase
    {
        private readonly IUsernameRepository _usernames;

        public DeleteViewModel()
        {
            _usernames = new UsernameRepository();
            CancelCommand = new RelayCommand(Cancel);
            ConfirmCommand = new RelayCommand(Confirm);
        }

        public Action Close { get; set; }

        public TwitchChannel Channel { get; set; }

        // 

        public ICommand CancelCommand { get; private set; }

        public ICommand ConfirmCommand { get; private set; }

        // 

        private void Cancel() => Close();

        private void Confirm() => Delete();

        private void Delete()
        {
            _usernames.RemoveUsername(Channel.Name);
            Messenger.Default.Send(ViewAction.Reset);
            Close();
        }
    }
}