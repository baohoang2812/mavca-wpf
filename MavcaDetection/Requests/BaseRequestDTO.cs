using System;
using System.Collections.Generic;
using System.Text;

namespace MavcaDetection.Requests
{
    public class BasePostRequestDTO
    {

    }

    public class BaseUpdateRequestDTO 
    {
        public string Id { get; set; }
    }

    public class BaseGetRequestDTO 
    {
        public int PageIndex { get; set; }
        public int Limit { get; set; }
        public string[] Columns { get; set; }
        public string[] Orders { get; set; }
        public int[] Ids { get; set; }
    }
}
