using System;
using System.Collections.Generic;
using System.Text;

namespace MavcaDetection.Response
{
    public class AuthResultDTO 
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public string RefreshToken { get; set; }
        public string RoleName { get; set; }
    }

    public class AuthResponse : BaseResponse<AuthResultDTO>
    {

    }
}
