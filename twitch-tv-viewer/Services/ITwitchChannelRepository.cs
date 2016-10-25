using System.Collections.Generic;
using System.Threading.Tasks;
using twitch_tv_viewer.Models;

namespace twitch_tv_viewer.Services
{
    internal interface ITwitchChannelRepository
    {
        Task<List<TwitchChannel>> GetChannels();
    }
}
