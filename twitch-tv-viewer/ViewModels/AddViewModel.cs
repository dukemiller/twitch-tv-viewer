using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight.CommandWpf;
using twitch_tv_viewer.Services;

namespace twitch_tv_viewer.ViewModels
{
    internal sealed class AddViewModel : INotifyPropertyChanged
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
                OnPropertyChanged();
            }
        }

        public Action Close { private get; set; }

        // 

        public RelayCommand CancelCommand { get; private set; }

        public RelayCommand ConfirmCommand { get; private set; }

        // 

        public event PropertyChangedEventHandler PropertyChanged;

        private void Cancel() => Close();

        private void Confirm() => Add();

        private void Add()
        {
            if (Name.Length > 0)
                _users.AddUsername(Name);
            Close();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}