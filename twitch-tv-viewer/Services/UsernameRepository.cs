using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using twitch_tv_viewer.Properties;

namespace twitch_tv_viewer.Services
{
    internal class UsernameRepository : IUsernameRepository
    {
        public UsernameRepository()
        {
            if (Settings.Default.Usernames == null)
                Settings.Default.Usernames = new StringCollection();
        }

        public IEnumerable<string> GetUsernames()
        {
            return Settings.Default.Usernames.Cast<string>().ToList();
        }

        public void AddUsername(string username)
        {
            Settings.Default.Usernames.Add(username.ToLower().Trim());
            Settings.Default.Save();
        }

        public void RemoveUsername(string username)
        {
            Settings.Default.Usernames.Remove(username.ToLower().Trim());
            Settings.Default.Save();
        }
    }
}