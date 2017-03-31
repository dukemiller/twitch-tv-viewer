namespace twitch_tv_viewer.Classes
{
    internal class Result
    {
        public bool Successful { get; set; }

        public bool Error { get; set; }

        public string Message { get; set; } = "";
    }
}
