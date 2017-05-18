using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.Practices.ServiceLocation;
using twitch_tv_viewer.Repositories.Interfaces;

namespace twitch_tv_viewer.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            if (AlreadyOpen)
            {
                FocusOtherViewerAndClose();
            }
            else
            {
                InitializeComponent();
                var settings = ServiceLocator.Current.GetInstance<ISettingsRepository>();
                Width = settings.Width;
                Height = settings.Height;
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

        private void FocusOtherViewerAndClose()
        {
            const int restore = 9;
            var hwnd = FindWindow(null, "TwitchTV Viewer");
            ShowWindow(hwnd, restore);
            SetForegroundWindow(hwnd);
            Close();
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var settings = ServiceLocator.Current.GetInstance<ISettingsRepository>();
            if (WindowState != WindowState.Maximized)
            {
                (var width, var height) = (e.NewSize.Width, e.NewSize.Height);
                settings.Width = Convert.ToInt16(width);
                settings.Height = Convert.ToInt16(height);
                settings.Save();
            }
        }
    }
}