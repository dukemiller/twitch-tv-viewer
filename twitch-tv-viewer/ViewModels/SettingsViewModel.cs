using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using twitch_tv_viewer.Repositories;

namespace twitch_tv_viewer.ViewModels
{
    internal class SettingsViewModel : ViewModelBase
    {
        private ObservableCollection<string> _items;

        private string _selected;

        private readonly ISettingsRepository _settings;

        // 

        public SettingsViewModel()
        {
            _settings = new SettingsRepository();
            Items = new ObservableCollection<string> {"Source", "Low"};
            Selected = _settings.Quality;
            ApplyCommand = new RelayCommand(Apply);
            CancelCommand = new RelayCommand(Cancel);
        }

        // 

        public Action Close { get; set; }

        public ObservableCollection<string> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                RaisePropertyChanged();
            }
        }

        public string Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                RaisePropertyChanged();
            }
        }

        public ICommand ApplyCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        // 

        private void Cancel() => Close();

        private void Apply()
        {
            _settings.Quality = Selected;
            Close();
        }
    }
}