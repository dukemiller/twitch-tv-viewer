using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using twitch_tv_viewer.Models;
using twitch_tv_viewer.Repositories;
using twitch_tv_viewer.ViewModels.Components;
using twitch_tv_viewer.Views;
using Add = twitch_tv_viewer.Views.Dialogs.Add;
using Edit = twitch_tv_viewer.Views.Dialogs.Edit;
using Settings = twitch_tv_viewer.Views.Dialogs.Settings;

namespace twitch_tv_viewer.ViewModels
{
    internal sealed class MainWindowViewModel : ViewModelBase
    {
        private readonly MessageDisplayViewModel _messageDisplay;

        private readonly ChannelsDisplayViewModel _channelsDisplay;

        private ViewModelBase _currentViewModel;

        private string _notification;

        private readonly ISettingsRepository _settings;

        // 

        public MainWindowViewModel()
        {
            Notification = "Loading ...";

            _settings = new SettingsRepository();
            
            _messageDisplay = new MessageDisplayViewModel();
            _channelsDisplay = new ChannelsDisplayViewModel();
            CurrentViewModel = _channelsDisplay;

            MessengerInstance.Register<NotificationMessage>(this, notification => Notification = notification.Message);
            MessengerInstance.Register<Result>(this, DisplayLogic);

            SettingsCommand = new RelayCommand(OpenSettings);
            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit);
            RefreshCommand = new RelayCommand(Refresh);
            SortCommand = new RelayCommand(Sort);
        }

        // 

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                RaisePropertyChanged();
            }
        }

        public string Notification
        {
            get { return _notification; }
            set
            {
                _notification = value;
                RaisePropertyChanged();
            }
        }
        
        private void DisplayLogic(Result result)
        {
            if (result.Successful)
                CurrentViewModel = _channelsDisplay;
            else
            {
                CurrentViewModel = _messageDisplay;
                _messageDisplay.Message = result.Message;
            }
        }

        public string SortName => _settings.SortName;

        // 

        public ICommand SettingsCommand { get; set; }

        public ICommand AddCommand { get; set; }

        public ICommand EditCommand { get; set; }

        public ICommand RefreshCommand { get; set; }

        public ICommand SortCommand { get; set; }

        // 

        private static void OpenSettings() => new Settings().ShowDialog();

        private async void Refresh()
        {
            // find a way to not allow multiple presses before refresh
            if (_channelsDisplay.Counter > 4)
            {
                Notification = "Refreshing ...";
                await Task.Delay(2000);
                MessengerInstance.Send(new ResetMessage());
            }
        }

        private void Sort()
        {
            _settings.SortBy = (_settings.SortBy + 1) % 4;
            RaisePropertyChanged(nameof(_settings.SortName));
            _channelsDisplay.Sort(_settings.SortName);
        }

        private static void Add() => new Add().ShowDialog();

        private static void Edit() => new Edit().ShowDialog();
    }
}