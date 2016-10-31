using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using twitch_tv_viewer.Models;
using twitch_tv_viewer.Repositories;

namespace twitch_tv_viewer.Services
{
    public class TwitchChannelService : ITwitchChannelService
    {
        public TwitchChannelService()
        {
            Settings = new SettingsRepository();
        }

        public ISettingsRepository Settings { get; set; }

        public async Task<string> PlayVideo(TwitchChannel channel, string quality)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "livestreamer",
                Arguments = $"--http-query-param client_id=spyiu9jqdnfjtwv6l1xjk5zgt8qb91l twitch.tv/{channel.Name} {quality}",
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

        public async Task<string> PlayVideo(TwitchChannel twitchChannel) => await PlayVideo(twitchChannel, Settings.Quality);

        public void OpenChat(TwitchChannel channel)
        {
            Process.Start($"http://twitch.tv/{channel.Name}/chat?popout=");
        }
    }
}
