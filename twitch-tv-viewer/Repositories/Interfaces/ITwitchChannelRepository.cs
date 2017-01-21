using System.Collections.Generic;
using System.Threading.Tasks;
using twitch_tv_viewer.Models;

namespace twitch_tv_viewer.Repositories
{
    internal interface ITwitchChannelRepository
    {
        Task<List<TwitchChannel>> GetChannels();
    }
}