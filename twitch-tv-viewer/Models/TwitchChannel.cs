using GalaSoft.MvvmLight;
using Newtonsoft.Json.Linq;

namespace twitch_tv_viewer.Models
{
    public class TwitchChannel: ObservableObject
    {
        private string _name;

        private string _viewers;

        private string _status;

        private string _game;

        private bool _promoted;

        // 

        public TwitchChannel()
        {}

        public TwitchChannel(JToken data)
        {
            var channel = data["channel"];
            Name = channel["display_name"]?.ToString() ?? "no name";
            Game = channel["game"]?.ToString() ?? "no game";
            Status = channel["status"]?.ToString().Trim() ?? "no status";
            Viewers = data["viewers"]?.ToString() ?? "???";
        }

        // 

        public string Name
        {
            get => _name;
            set => Set(() => Name, ref _name, value);
        }

        public string Game
        {
            get => _game;
            set => Set(() => Game, ref _game, value);
        }

        public string Status
        {
            get => _status;
            set => Set(() => Status, ref _status, value);
        }

        public string Viewers
        {
            get => _viewers;
            set => Set(() => Viewers, ref _viewers, value);
        }

        public bool Promoted
        {
            get => _promoted;
            set => Set(() => Promoted, ref _promoted, value);
        }
    }
}