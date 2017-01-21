using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using twitch_tv_viewer.Repositories;
using twitch_tv_viewer.Services;
using twitch_tv_viewer.ViewModels;
using twitch_tv_viewer.ViewModels.Components;
using twitch_tv_viewer.ViewModels.Dialogs;

namespace twitch_tv_viewer
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Repositories
            SimpleIoc.Default.Register<ISettingsRepository, SettingsRepository>();
            SimpleIoc.Default.Register<ITwitchChannelRepository, TwitchChannelRepository>();
            SimpleIoc.Default.Register<IUsernameRepository, UsernameRepository>();

            // Services
            SimpleIoc.Default.Register<ISoundPlayerService, SoundPlayerService>();
            SimpleIoc.Default.Register<ITwitchChannelService, TwitchChannelService>();

            // Viewmodels
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<ChannelsDisplayViewModel>();
            SimpleIoc.Default.Register<MessageDisplayViewModel>();

            // Dialog viewmodels
            SimpleIoc.Default.Register<AddViewModel>();
            SimpleIoc.Default.Register<DeleteViewModel>();
            SimpleIoc.Default.Register<EditViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
        }

        public MainWindowViewModel Main => ServiceLocator.Current.GetInstance<MainWindowViewModel>();
    }
}
