using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using twitch_tv_viewer.Models;

namespace twitch_tv_viewer.Services
{
    internal class TwitchChannelRepository : ITwitchChannelRepository
    {
        private readonly HttpClient _client;
        private readonly UsernameRepository _usernames;

        public TwitchChannelRepository()
        {
            _client = new HttpClient();
            _usernames = new UsernameRepository();
        }

        public async Task<List<TwitchChannel>> GetChannels()
        {
            try
            {
                var request = CreateRequest(GetChannelUrl());
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

        private string GetChannelUrl()
        {
            return "https://api.twitch.tv/kraken/streams?channel=" + string.Join(",", _usernames.GetUsernames());
        }

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
            var json = (JObject) await Task.Factory.StartNew(() => JsonConvert.DeserializeObject(content));
            var streamers = json["streams"].Select(s => new TwitchChannel(s));
            return streamers.OrderBy(c => c).ToList();
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