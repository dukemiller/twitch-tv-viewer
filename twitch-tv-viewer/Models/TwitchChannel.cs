using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace twitch_tv_viewer.Models
{
    public class TwitchChannel : IComparable
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

        public int CompareTo(object obj)
        {
            var that = obj as TwitchChannel;
            if (that == null)
                return 0;
            return string.Compare(Name.ToLower(), that.Name.ToLower(), StringComparison.Ordinal);
        }
        
    }
}