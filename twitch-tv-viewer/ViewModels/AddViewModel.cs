using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using twitch_tv_viewer.Repositories;

namespace twitch_tv_viewer.ViewModels
{
    internal sealed class AddViewModel : ViewModelBase
    {
        private readonly UsernameRepository _users;
        private string _name;

        public AddViewModel()
        {
            _users = new UsernameRepository();
            _name = "";
            CancelCommand = new RelayCommand(Cancel);
            ConfirmCommand = new RelayCommand(Confirm);
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        public Action Close { private get; set; }

        // 

        public RelayCommand CancelCommand { get; private set; }

        public RelayCommand ConfirmCommand { get; private set; }

        // 

        private void Cancel() => Close();

        private void Confirm() => Add();

        private void Add()
        {
            if (Name.Length > 0)
            {
                _users.AddUsername(Name);
                Messenger.Default.Send(new ResetMessage());
            }
            Close();
        }
    }
}