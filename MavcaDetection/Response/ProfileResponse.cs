using System;
using System.Collections.Generic;
using System.Text;

namespace MavcaDetection.Response
{
    public class EmployeeData
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public BranchData Branch { get; set; }
    }
    public class BranchData
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ProfileResponse : BaseResponse<EmployeeData>
    {
    }
}
