using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using twitch_tv_viewer.ViewModels.Components;
using twitch_tv_viewer.Views;

namespace twitch_tv_viewer.ViewModels
{
    internal sealed class MainWindowViewModel : ViewModelBase
    {
        private readonly MessageDisplayViewModel _messageDisplay;

        private readonly ChannelsDisplayViewModel _channelsDisplay;

        private ViewModelBase _currentViewModel;

        private string _notification;

        // 

        public MainWindowViewModel()
        {
            Notification = "Loading ...";
            
            _messageDisplay = new MessageDisplayViewModel();
            _channelsDisplay = new ChannelsDisplayViewModel();
            CurrentViewModel = _channelsDisplay;

            MessengerInstance.Register<NotificationMessage>(this, notification => Notification = notification.Message);
            MessengerInstance.Register<Result>(this, DisplayLogic);

            SettingsCommand = new RelayCommand(OpenSettings);
            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit);
            RefreshCommand = new RelayCommand(Refresh);
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

        // 

        public RelayCommand SettingsCommand { get; set; }

        public RelayCommand AddCommand { get; set; }

        public RelayCommand EditCommand { get; set; }

        public RelayCommand RefreshCommand { get; set; }

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

        private static void Add() => new Add().ShowDialog();

        private static void Edit() => new Edit().ShowDialog();
    }
}