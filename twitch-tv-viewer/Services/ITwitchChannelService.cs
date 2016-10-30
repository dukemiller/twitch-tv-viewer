using System.Threading.Tasks;
using twitch_tv_viewer.Models;

namespace twitch_tv_viewer.Services
{
    interface ITwitchChannelService
    {
        Task<string> PlayVideo(TwitchChannel twitchChannel);
        Task<string> PlayVideo(TwitchChannel twitchChannel, string quality);
        void OpenChat(TwitchChannel twitchChannel);
    }
}
