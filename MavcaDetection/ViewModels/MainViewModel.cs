using MavcaDetection.Constants;
using MavcaDetection.Services;
using MavcaDetection.Views;
using System.Windows;
using System.Windows.Input;

namespace MavcaDetection.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public bool IsLoaded { get; set; } = false;
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand DetectCommand { get; set; }
        public ICommand LoadConfigCommand { get; set; }
        public MainViewModel()
        {
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
                //pythonService.Source = @"C:\Users\PC\Desktop\Capstone\mavca-detect\main.py";
                //if (!pythonService.RunScript(null))
                //{
                //    new MessageBoxCustom("Script file not found!", MessageType.Error, MessageButtons.Ok).ShowDialog();
                //}
                pythonService.RunScript();
            });

            LoadConfigCommand = new RelayCommand<object>(p => true, p =>
            {

            });
        }
    }
}
