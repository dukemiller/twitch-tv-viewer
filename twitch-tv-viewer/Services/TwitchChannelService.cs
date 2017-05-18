using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using twitch_tv_viewer.Models;
using twitch_tv_viewer.Repositories.Interfaces;
using twitch_tv_viewer.Services.Interfaces;

namespace twitch_tv_viewer.Services
{
    public class TwitchChannelService : ITwitchChannelService
    {
        private readonly HttpClient _client;

        private readonly ISettingsRepository _settings;

        // 

        public TwitchChannelService(ISettingsRepository settings)
        {
            _settings = settings;
            _client = new HttpClient { Timeout = new TimeSpan(0, 0, 3) };
        }

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

        public async Task<List<TwitchChannel>> GetChannels()
        {
            try
            {
                var request = CreateRequest(ChannelsUrl());
                var response = await _client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                    return await ParseData(response);

                return new List<TwitchChannel>();
            }

            catch (Exception)
            {
                return new List<TwitchChannel>();
            }
        }

        // 

        private string ChannelsUrl() => "https://api.twitch.tv/kraken/streams?channel=" + string.Join(",", _settings.Usernames);

        public static Task<List<TwitchChannel>> GetChannelsStatic()
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

        private static async Task<List<TwitchChannel>> ParseData(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var json = (JObject)await Task.Factory.StartNew(() => JsonConvert.DeserializeObject(content));
            var streamers = json["streams"].Select(s => new TwitchChannel(s));
            return streamers.ToList();
        }

        private static HttpRequestMessage CreateRequest(string streamUrl)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(streamUrl)
            };
            request.Headers.Add("Client-Id", "spyiu9jqdnfjtwv6l1xjk5zgt8qb91l");
            return request;
        }
    }
}
