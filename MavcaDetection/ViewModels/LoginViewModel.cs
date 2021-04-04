using MavcaDetection.Constants;
using MavcaDetection.Response;
using MavcaDetection.Services;
using MavcaDetection.Views;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MavcaDetection.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public bool IsLogin { get; set; }
        public string AccessToken { get; set; }
        public int BranchId { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }

        #region Property
        private string _Username;
        public string Username
        {
            get { return _Username; }
            set
            {
                _Username = value;
                OnPropertyChanged();
            }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand<Window>(p => true, p =>
            {
                Login(p);
            });
            PasswordChangedCommand = new RelayCommand<PasswordBox>(p => true, p =>
            {
                Password = p.Password;
            });
            ClearCommand = new RelayCommand<PasswordBox>(p => true, p =>
            {
                Username = null;
                p.Password = null;
            });
        }

        private async void Login(Window w)
        {
            if (w == null)
                return;
            var authService = new AuthService();
            var response = await authService.Login(Username, Password);
            if (response.IsSuccessStatusCode && HttpStatusCode.OK == response.StatusCode)
            {
                var result = await authService.ReadResponse<AuthResponse>(response);
                var data = result.Data;
                if (data != null)
                {
                    AccessToken = data.AccessToken;
                }
                var profileService = new EmployeeService();
                profileService.SetAuthorizationHeader(AccessToken);
                var profileResult = await profileService.Get<ProfileResponse>();
                if (profileResult != null)
                {
                    BranchId = profileResult.Data.Branch.Id;
                }
                IsLogin = true;
                w.Close();
            }
            else
            {
                IsLogin = false;
                new MessageBoxCustom("Invalid Username or Password", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            }
        }
    }
}
