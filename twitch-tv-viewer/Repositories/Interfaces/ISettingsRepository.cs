﻿namespace twitch_tv_viewer.Repositories
{
    public interface ISettingsRepository
    {
        string Quality { get; set; }
        bool UserAlert { get; set; }
        int SortBy { get; set; }
        string SortName { get; }
    }
}