using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unit_Testing_xUnit_ASP_NET_Core.Models
{
    public class modelUser
    {
        public string username { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public string email { get; set; }
        public string token { get; set; }
        public DateTime expiredToken { get; set; }
        public bool online { get; set; }
    }
    
    public class modelInputUser
    {
        public string username { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public string email { get; set; }
    }
}
