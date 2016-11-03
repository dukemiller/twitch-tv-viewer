namespace twitch_tv_viewer.Repositories
{
    internal class SettingsRepository: ISettingsRepository
    {
        public string Quality
        {
            get { return Properties.Settings.Default.Quality; }
            set
            {
                Properties.Settings.Default.Quality = value;
                Properties.Settings.Default.Save();
            }
        }

        public bool UserAlert
        {
            get { return Properties.Settings.Default.UserAlert; }
            set
            {
                Properties.Settings.Default.UserAlert = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}
