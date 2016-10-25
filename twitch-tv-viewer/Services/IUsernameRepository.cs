using System.Collections.Generic;

namespace twitch_tv_viewer.Services
{
    internal interface IUsernameRepository
    {
        List<string> GetUsernames();
        void AddUsername(string username);
        void RemoveUsername(string username);
    }
}
