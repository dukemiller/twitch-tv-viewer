namespace twitch_tv_viewer.Repositories
{
    internal class SettingsRepository: ISettingsRepository
    {
        public string Quality
        {
            get { return Properties.Settings.Default.Quality; }
            set { Properties.Settings.Default.Quality = value; }
        }
    }
}
