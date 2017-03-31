using System.Media;
using System.Reflection;

namespace twitch_tv_viewer.Services
{
    internal class SoundPlayerService: ISoundPlayerService
    {
        private readonly Assembly _assembly;

        public SoundPlayerService()
        {
            _assembly = Assembly.GetExecutingAssembly();
        }

        public void PlayOnlineSound()
        {
            using (var online = _assembly.GetManifestResourceStream("twitch_tv_viewer.Resources.Sfx.online.wav"))
                using (var player = new SoundPlayer(online))
                    player.Play();
        }

        public void PlayOfflineSound()
        {
            using (var offline = _assembly.GetManifestResourceStream("twitch_tv_viewer.Resources.Sfx.offline.wav"))
                using (var player = new SoundPlayer(offline))
                    player.Play();
        }
    }
}
