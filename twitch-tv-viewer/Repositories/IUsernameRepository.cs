using System.Collections.Generic;

namespace twitch_tv_viewer.Repositories
{
    internal interface IUsernameRepository
    {
        IEnumerable<string> GetUsernames();
        void AddUsername(string username);
        void RemoveUsername(string username);
    }
}