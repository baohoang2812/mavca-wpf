using System;
using System.Collections.Generic;
using System.Text;

namespace MavcaDetection.Services
{
    public class EmployeeService : BaseService
    {
        public EmployeeService()
        {
            EndPoint = new Uri($"{BaseURL}employees/profile");
        }
    }
}
