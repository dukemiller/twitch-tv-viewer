using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using twitch_tv_viewer.Enums;
using twitch_tv_viewer.Repositories;

namespace twitch_tv_viewer.ViewModels.Dialogs
{
    internal sealed class AddViewModel : ViewModelBase
    {
        private readonly IUsernameRepository _users;

        private string _name;

        public AddViewModel()
        {
            _users = SimpleIoc.Default.GetInstance<IUsernameRepository>();
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
            if (Name.Length > 0)
            {
                _users.AddUsername(Name);
                Messenger.Default.Send(ViewAction.Reset);
            }
            Close();
        }
    }
}