using System.Threading.Tasks;
using twitch_tv_viewer.Models;

namespace twitch_tv_viewer.Services.Interfaces
{
    public interface ITwitchChannelService
    {
        Task<string> PlayVideo(TwitchChannel channel);
        Task<string> PlayVideo(TwitchChannel channel, string quality);
        void OpenChat(TwitchChannel twitchChannel);
    }
}
