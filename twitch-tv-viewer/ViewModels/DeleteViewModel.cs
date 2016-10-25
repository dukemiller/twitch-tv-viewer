﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using twitch_tv_viewer.Models;
using twitch_tv_viewer.Services;

namespace twitch_tv_viewer.ViewModels
{
    internal sealed class DeleteViewModel : INotifyPropertyChanged
    {

        public DeleteViewModel()
        {
            CancelCommand = new RelayCommand(Cancel);
            ConfirmCommand = new RelayCommand(Confirm);
            
        }

        public Action Close { get; set;  }

        public TwitchChannel Channel { get; set; }

        // 

        public RelayCommand CancelCommand { get; private set; }

        public RelayCommand ConfirmCommand { get; private set; }
        
        private void Cancel() => Close();

        private void Confirm() => Close();

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}