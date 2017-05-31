namespace twitch_tv_viewer.Services.Interfaces
{
    public interface ISoundPlayerService
    {
        void PlayOnlineSound();
        void PlayOfflineSound();
        void PlayAnnouncement();
    }
}