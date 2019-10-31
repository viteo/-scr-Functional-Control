using Sharpsaver.Models;
using System;
using System.Windows;
using System.Windows.Input;

namespace Sharpsaver.ViewModels
{
    public class SettingsViewModel : ObservableObject
    {
        public Layout Layout
        {
            get => Settings.Instance.Layout;
            set
            {
                Settings.Instance.Layout = value;
                OnPropertyChanged("Layout");
            }
        }

        public int BrickSize
        {
            get => Settings.Instance.BrickSize;
            set
            {
                Settings.Instance.BrickSize = value;
                OnPropertyChanged("BrickSize");
            }
        }

        public double SwitchPeriod
        {
            get => Settings.Instance.SwitchPeriod;
            set
            {
                Settings.Instance.SwitchPeriod = value;
                OnPropertyChanged("SwitchPeriod");
            }
        }

        public bool ShowMagicNumber
        {
            get => Settings.Instance.ShowMagicNumber;
            set
            {
                Settings.Instance.ShowMagicNumber = value;
                OnPropertyChanged("ShowMagicNumber");
            }
        }

        public bool IsFullscreen
        {
            get => Settings.Instance.IsFullscreen;
            set
            {
                Settings.Instance.IsFullscreen = value;
                OnPropertyChanged("IsFullscreen");
            }
        }

        public SettingsViewModel()
        {
            Settings.Instance = new Settings();
            Settings.Instance.LoadSettings();
        }

        public ICommand SaveCommand
        {
            get { return new DelegateCommand(new Action<object>(Save)); }
        }

        public void Save(object obj)
        {
            Settings.Instance.SaveSettings();
            Application.Current.Shutdown();
        }

    }
}
