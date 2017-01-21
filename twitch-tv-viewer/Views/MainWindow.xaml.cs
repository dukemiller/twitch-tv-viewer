using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;

namespace twitch_tv_viewer.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            if (AlreadyOpen)
            {
                FocusOtherDownloaderAndClose();
            }
            else
            {
                InitializeComponent();
            }
        }

        private static bool AlreadyOpen => Process
            .GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location))
            .Length > 1;

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string sClassName, string sAppName);

        private void FocusOtherDownloaderAndClose()
        {
            const int restore = 9;
            var hwnd = FindWindow(null, "TwitchTV Viewer");
            ShowWindow(hwnd, restore);
            SetForegroundWindow(hwnd);
            Close();
        }
    }
}