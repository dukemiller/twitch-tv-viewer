using System.Threading.Tasks;
using twitch_tv_viewer.Models;

namespace twitch_tv_viewer.Services
{
    interface ITwitchChannelService
    {
        Task<string> PlayVideo(TwitchChannel twitchChannel);
        void OpenChat(TwitchChannel twitchChannel);
    }
}
