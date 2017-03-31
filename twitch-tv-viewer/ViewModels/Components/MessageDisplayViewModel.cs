using GalaSoft.MvvmLight;

namespace twitch_tv_viewer.ViewModels.Components
{
    internal class MessageDisplayViewModel : ViewModelBase
    {
        private string _message;

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                RaisePropertyChanged();
                MessengerInstance.Send("");
            }
        }
    }
}
