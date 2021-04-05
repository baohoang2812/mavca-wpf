using MavcaDetection.Constants;
using MavcaDetection.Services;
using MavcaDetection.Views;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MavcaDetection.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public bool IsLoaded { get; set; } = false;
        public bool IsStarted { get; set; } = false;
        private string _StartButtonName;
        public string StartButtonName
        {
            get { return _StartButtonName; }
            set
            {
                _StartButtonName = value;
                OnPropertyChanged();
            }
        }
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand DetectCommand { get; set; }
        public ICommand LoadConfigCommand { get; set; }
        public MainViewModel()
        {
            StartButtonName = "Start Detect";
            LoadedWindowCommand = new RelayCommand<Window>(p => true, p =>
            {
                IsLoaded = true;
                if (p == null)
                   return;
                p.Hide();
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
                if(loginWindow.DataContext == null)
                   return;
                var loginVM = loginWindow.DataContext as LoginViewModel;
                if (loginVM.IsLogin)
                {
                    p.Show();
                }
                else
                {
                    p.Close();
                }
            });

            DetectCommand = new RelayCommand<object>(p => true, p =>
            {
                var pythonService = new PyService();
                if (!IsStarted)
                {
                    StartButtonName = "Stop";
                    IsStarted = true;
                    //pythonService.RunScript();
                    new MessageBoxCustom("Detection started", MessageType.Success, MessageButtons.Ok).ShowDialog();
                }
                else
                {
                    var result = new MessageBoxCustom("Are you sure to stop?", MessageType.Info, MessageButtons.YesNo).ShowDialog();
                    if(result != null && result.Value == true)
                    {
                        IsStarted = false;
                        StartButtonName = "Start Detect";
                        new MessageBoxCustom("Detection stopped", MessageType.Success, MessageButtons.Ok).ShowDialog();
                    }
                }
            });

            LoadConfigCommand = new RelayCommand<object>(p => true, p =>
            {
                var dialog = new OpenFileDialog();
                dialog.DefaultExt = ".json";
                dialog.Filter = "Json files (*.json)|*.json";
                var result = dialog.ShowDialog();
                if(result == true)
                {
                    string filename = dialog.FileName;
                    new MessageBoxCustom("Load Configuration Success", MessageType.Success, MessageButtons.Ok).ShowDialog();
                }
            });
        }
    }
}
