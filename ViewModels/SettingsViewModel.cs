using Sharpsaver.Models;
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

        public int BrickSize
        {
            get => Settings.Instance.BrickSize;
            set
            {
                Settings.Instance.BrickSize = value;
                OnPropertyChanged();
            }
        }

        public double SwitchPeriod
        {
            get => Settings.Instance.SwitchPeriod;
            set
            {
                Settings.Instance.SwitchPeriod = value;
                OnPropertyChanged();
            }
        }

        public bool ShowMagicNumber
        {
            get => Settings.Instance.ShowMagicNumber;
            set
            {
                Settings.Instance.ShowMagicNumber = value;
                OnPropertyChanged();
            }
        }

        public bool IsFullscreen
        {
            get => Settings.Instance.IsFullscreen;
            set
            {
                Settings.Instance.IsFullscreen = value;
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
