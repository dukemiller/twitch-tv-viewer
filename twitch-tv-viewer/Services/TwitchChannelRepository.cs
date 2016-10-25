using System.Collections.Generic;
using System.Threading.Tasks;
using twitch_tv_viewer.Models;

namespace twitch_tv_viewer.Services
{
    internal class TwitchChannelRepository : ITwitchChannelRepository
    {
        public Task<List<TwitchChannel>> GetChannels()
        {
            var collection = new List<TwitchChannel>();

            collection.Add(new TwitchChannel
            {
                Game = "Dota 2",
                Name = "Arteezy",
                Status = "Playing games",
                Viewers = "19248"
            });


            collection.Add(new TwitchChannel
            {
                Game = "League of Legends",
                Name = "Destiny",
                Status = "Watch me dive 1v4 and rage",
                Viewers = "200"
            });

            collection.Add(new TwitchChannel
            {
                Game = "Banjo-Kazooie",
                Name = "Stivitybobo",
                Status = "I dont like this game anymore",
                Viewers = "852"
            });

            return Task.Run(() => collection);
        }
    }
}
