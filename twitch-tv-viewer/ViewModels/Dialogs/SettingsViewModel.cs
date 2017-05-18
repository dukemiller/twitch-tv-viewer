using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using twitch_tv_viewer.Enums;
using twitch_tv_viewer.Repositories.Interfaces;

namespace twitch_tv_viewer.ViewModels.Dialogs
{
    public class SettingsViewModel : ViewModelBase
    {
        private ObservableCollection<string> _items;

        private string _selected;

        private readonly ISettingsRepository _settings;

        private bool _checked;

        // 

        public SettingsViewModel(ISettingsRepository settings)
        {
            _settings = settings;
            Items = new ObservableCollection<string> {"source,best,1080p", "720p,720p30", "480p,low"};
            Selected = _settings.Quality;
            Checked = _settings.UserAlert;
            ApplyCommand = new RelayCommand(Apply);
            CancelCommand = new RelayCommand(Cancel);
        }

        // 

        public Action Close { get; set; }

        public ObservableCollection<string> Items
        {
            get => _items;
            set => Set(() => Items, ref _items, value);
        }

        public string Selected
        {
            get => _selected;
            set => Set(() => Selected, ref _selected, value);
        }

        public bool Checked
        {
            get => _checked;
            set => Set(() => Checked, ref _checked, value);
        }

        public ICommand ApplyCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        // 

        private void Cancel() => Close();

        private void Apply()
        {
            if (_settings.UserAlert != Checked || !_settings.Quality.Equals(Selected))
                Messenger.Default.Send((MessageType.Notification, "Changed settings."));
            _settings.UserAlert = Checked;
            _settings.Quality = Selected;
            _settings.Save();
            Close();
        }
    }
}