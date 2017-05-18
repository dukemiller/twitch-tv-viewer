﻿using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace twitch_tv_viewer.Repositories.Interfaces
{
    public interface ISettingsRepository
    {
        string Quality { get; set; }

        bool UserAlert { get; set; }

        int SortBy { get; set; }

        string SortName { get; }

        int Width { get; set; }

        int Height { get; set; }

        ObservableCollection<string> Usernames { get; set; }

        Task Save();
    }
}
