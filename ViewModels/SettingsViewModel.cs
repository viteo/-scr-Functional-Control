using Sharpsaver.Models;
using System;
using System.Windows;
using System.Windows.Input;

namespace Sharpsaver.ViewModels
{
    public class SettingsViewModel : ObservableObject
    {

        public Param1 Parameter1
        {
            get => Settings.Instance.param1;
            set
            {
                Settings.Instance.param1 = value;
                OnPropertyChanged();
            }
                
        }

        public Param2 Parameter2
        {
            get => Settings.Instance.param2;
            set
            {
                Settings.Instance.param2 = value;
                OnPropertyChanged();
            }
        }

        public int Parameter3
        {
            get => Settings.Instance.param3;
            set
            {
                Settings.Instance.param3 = value;
                OnPropertyChanged();
            }
        }

        public bool Parameter4
        {
            get => Settings.Instance.param4;
            set
            {
                Settings.Instance.param4 = value;
                OnPropertyChanged();
            }
        }

        public SettingsViewModel()
        {
            Settings.Instance = new Settings();
            Settings.Instance.LoadSettings();
        }

        public ICommand SaveCommand
        {
            get { return new DelegateCommand(Save); }
        }

        public void Save()
        {
            Settings.Instance.SaveSettings();
            Application.Current.Shutdown();
        }

    }
}
