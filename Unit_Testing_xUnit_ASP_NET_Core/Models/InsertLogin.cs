using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unit_Testing_xUnit_ASP_NET_Core.Models
{
    public class InsertLogin
    {
        public string username { get; set; }
        public bool online { get; set; }
        public string token { get; set; }
        public DateTime expiredToken { get; set; }
    }
}
