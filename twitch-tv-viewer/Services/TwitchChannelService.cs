using System.Diagnostics;
using System.Threading.Tasks;
using twitch_tv_viewer.Models;
using twitch_tv_viewer.Repositories;
using twitch_tv_viewer.Services.Interfaces;

namespace twitch_tv_viewer.Services
{
    public class TwitchChannelService : ITwitchChannelService
    {
        public TwitchChannelService(ISettingsRepository settings) => _settings = settings;

        private readonly ISettingsRepository _settings;

        public async Task<string> PlayVideo(TwitchChannel channel, string quality)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "streamlink",
                Arguments = $"twitch.tv/{channel.Name} {quality}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            var process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();

            return await process.StandardOutput.ReadToEndAsync();
        }

        public async Task<string> PlayVideo(TwitchChannel channel) => await PlayVideo(channel, _settings.Quality);

        public void OpenChat(TwitchChannel channel) => Process.Start($"http://twitch.tv/{channel.Name}/chat?popout=");

        public void OpenStream(TwitchChannel channel) => Process.Start($"http://twitch.tv/{channel.Name}");
    }
}
