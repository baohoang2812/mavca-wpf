using MavcaDetection.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MavcaDetection.Requests
{
    public class LoginRequestDTO : BasePostRequestDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
