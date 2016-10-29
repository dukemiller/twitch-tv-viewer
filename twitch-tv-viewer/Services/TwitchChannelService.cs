using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using twitch_tv_viewer.Models;

namespace twitch_tv_viewer.Services
{
    public class TwitchChannelService : ITwitchChannelService
    {
        public async Task<string> PlayVideo(TwitchChannel channel)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "livestreamer",
                Arguments = $"--http-query-param client_id=spyiu9jqdnfjtwv6l1xjk5zgt8qb91l twitch.tv/{channel.Name} source",
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

        public void OpenChat(TwitchChannel channel)
        {
            Process.Start($"http://twitch.tv/{channel.Name}/chat?popout=");
        }
    }
}
