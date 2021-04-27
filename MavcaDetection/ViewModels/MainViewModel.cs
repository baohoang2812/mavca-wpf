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
        public bool IsHandDetectionEnabled { get; set; } = false;
        public bool IsPhoneDetectionEnabled { get; set; } = false;
        public bool IsTablewareDetectionEnabled { get; set; } = false;
        private string _EnableTableSetupDetectionButtonName;
        public string EnableTableSetupDetectionButtonName
        {
            get { return _EnableTableSetupDetectionButtonName; }
            set
            {
                _EnableTableSetupDetectionButtonName = value;
                OnPropertyChanged();
            }
        }

        private string _EnableBareHandDetectionButtonName;
        public string EnableBareHandDetectionButtonName
        {
            get { return _EnableBareHandDetectionButtonName; }
            set
            {
                _EnableBareHandDetectionButtonName = value;
                OnPropertyChanged();
            }
        }

        private string _EnablePhoneDetectionButtonName;
        public string EnablePhoneDetectionButtonName
        {
            get { return _EnablePhoneDetectionButtonName; }
            set
            {
                _EnablePhoneDetectionButtonName = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadedWindowCommand { get; set; }
        public ICommand DetectTablewareCommand { get; set; }
        public ICommand DetectHandCommand { get; set; }
        public ICommand DetectPhoneCommand { get; set; }
        public ICommand LoadConfigCommand { get; set; }
        public MainViewModel()
        {
            EnableTableSetupDetectionButtonName = "Enable Tableware Detection";
            EnableBareHandDetectionButtonName = "Enable Hand Detection";
            EnablePhoneDetectionButtonName = "Enable Phone Detection";
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

            DetectTablewareCommand = new RelayCommand<object>(p => true, p =>
            {
                var pythonService = new PyService();
                if (!IsTablewareDetectionEnabled)
                {
                    EnableTableSetupDetectionButtonName = "Disable Tableware Detection";
                    IsTablewareDetectionEnabled = true;
                    pythonService.RunScript(DetectionTypeConstant.TableWare);
                    new MessageBoxCustom("Tableware Detection started", MessageType.Success, MessageButtons.Ok).ShowDialog();
                }
                else
                {
                    var result = new MessageBoxCustom("Are you sure to stop?", MessageType.Info, MessageButtons.YesNo).ShowDialog();
                    if (result != null && result.Value == true)
                    {
                        IsTablewareDetectionEnabled = false;
                        EnableTableSetupDetectionButtonName = "Enable Tableware Detection";
                        new MessageBoxCustom("Tableware Detection stopped", MessageType.Success, MessageButtons.Ok).ShowDialog();
                    }
                }
            });

            DetectHandCommand = new RelayCommand<object>(p => true, p =>
            {
                var pythonService = new PyService();
                if (!IsHandDetectionEnabled)
                {
                    EnableBareHandDetectionButtonName = "Disable Hand Detection";
                    IsHandDetectionEnabled = true;
                    pythonService.RunScript(DetectionTypeConstant.Hand);
                    new MessageBoxCustom("Hand Detection enabled", MessageType.Success, MessageButtons.Ok).ShowDialog();
                }
                else
                {
                    var result = new MessageBoxCustom("Are you sure to stop?", MessageType.Info, MessageButtons.YesNo).ShowDialog();
                    if(result != null && result.Value == true)
                    {
                        IsHandDetectionEnabled = false;
                        EnableTableSetupDetectionButtonName = "Enable Hand Detect";
                        new MessageBoxCustom("Hand Detection stopped", MessageType.Success, MessageButtons.Ok).ShowDialog();
                    }
                }
            });

            DetectPhoneCommand = new RelayCommand<object>(p => true, p =>
            {
                var pythonService = new PyService();
                if (!IsPhoneDetectionEnabled)
                {
                    EnablePhoneDetectionButtonName = "Disable Phone Detection";
                    IsPhoneDetectionEnabled = true;
                    pythonService.RunScript(DetectionTypeConstant.Phone);
                    new MessageBoxCustom("Phone Detection enabled", MessageType.Success, MessageButtons.Ok).ShowDialog();
                }
                else
                {
                    var result = new MessageBoxCustom("Are you sure to stop?", MessageType.Info, MessageButtons.YesNo).ShowDialog();
                    if (result != null && result.Value == true)
                    {
                        IsPhoneDetectionEnabled = false;
                        EnablePhoneDetectionButtonName = "Enable Phone Detection";
                        new MessageBoxCustom("Phone Detection disabled", MessageType.Success, MessageButtons.Ok).ShowDialog();
                    }
                }
            });

            LoadConfigCommand = new RelayCommand<object>(p => true, p =>
            {
                var dialog = new OpenFileDialog();
                dialog.DefaultExt = ".zip";
                dialog.Filter = "ZIP|*.zip";
                var result = dialog.ShowDialog();
                if(result == true)
                {
                    string filename = dialog.FileName;
                    var pythonService = new PyService();
                    if (pythonService.LoadConfiguration(filename))
                    {
                        new MessageBoxCustom("Load Configuration Success", MessageType.Success, MessageButtons.Ok).ShowDialog();
                    }
                    else
                    {
                        new MessageBoxCustom("Invalid configuration file", MessageType.Error, MessageButtons.Ok).ShowDialog();
                    }
                }
            });
        }
    }
}
