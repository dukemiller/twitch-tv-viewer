using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace twitch_tv_viewer.Repositories.Interfaces
{
    public interface ISettingsRepository
    {
        string Quality { get; set; }

        bool UserAlert { get; set; }

        int SortBy { get; set; }

        string SortName { get; }

        ObservableCollection<string> Usernames { get; set; }

        Task Save();
    }
}
