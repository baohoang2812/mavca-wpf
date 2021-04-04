using MavcaDetection.Requests;
using MavcaDetection.Response;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MavcaDetection.Services
{
    public class AuthService : BaseService
    {
        public AuthService()
        {
            EndPoint = new Uri($"{BaseURL}auth/login");
        }

        public async Task<HttpResponseMessage> Login(string username, string password)
        {
            var loginRequest = new LoginRequestDTO
            {
                Username = username,
                Password = password
            };
            return await base.PostAsync(loginRequest);
        }
    }
}
