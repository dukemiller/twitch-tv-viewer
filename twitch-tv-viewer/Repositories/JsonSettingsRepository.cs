using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using twitch_tv_viewer.Models;
using twitch_tv_viewer.Repositories.Interfaces;
using twitch_tv_viewer.ViewModels;

namespace twitch_tv_viewer.Repositories
{
    public class JsonSettingsRepository: ISettingsRepository
    {
        [JsonProperty("quality")]
        public string Quality { get; set; } = "source,best,1080p";

        [JsonProperty("alert_user")]
        public bool UserAlert { get; set; } = false;

        [JsonProperty("sort_property_int")]
        public int SortBy { get; set; } = 0;

        [JsonProperty("sort_property")]
        public string SortName => TypeDescriptor.GetProperties(typeof(TwitchChannel))[SortBy].Name;

        [JsonProperty("usernames")]
        public ObservableCollection<string> Usernames { get; set; } = new ObservableCollection<string>();

        [JsonIgnore]
        private static readonly string SavePath = Path.Combine(MainWindowViewModel.ApplicationPath, "settings.json");

        public async Task Save()
        {
            using (var stream = new StreamWriter(SavePath))
                await stream.WriteAsync(JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public static JsonSettingsRepository Load()
        {
            if (!Directory.Exists(MainWindowViewModel.ApplicationPath))
                Directory.CreateDirectory(MainWindowViewModel.ApplicationPath);

            if (File.Exists(SavePath))
                using (var stream = new StreamReader(SavePath))
                    return JsonConvert.DeserializeObject<JsonSettingsRepository>(stream.ReadToEnd());

            return new JsonSettingsRepository();
        }
       
    }
}