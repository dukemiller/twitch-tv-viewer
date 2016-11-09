using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace twitch_tv_viewer.Models
{
    public class TwitchChannel
    {
        public TwitchChannel()
        {
        }

        public TwitchChannel(JToken data)
        {
            var channel = data["channel"];
            Name = channel["display_name"]?.ToString() ?? "no name";
            Game = channel["game"]?.ToString() ?? "no game";
            Status = channel["status"]?.ToString().Trim() ?? "no status";
            Viewers = data["viewers"]?.ToString() ?? "???";
        }

        public string Name { get; set; }

        public string Game { get; set; }

        public string Status { get; set; }

        public string Viewers { get; set; }
        
    }
}