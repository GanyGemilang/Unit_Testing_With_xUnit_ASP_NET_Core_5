using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unit_Testing_xUnit_ASP_NET_Core.Utilities
{
    public class Time
    {
        public static DateTime timezone(DateTime input)
        {

            try
            {
                TimeZoneInfo timeZoneInfo;
                DateTime dateTime;
                //Set the time zone information to US Mountain Standard Time 
                timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                //Get date and time in US Mountain Standard Time 
                dateTime = TimeZoneInfo.ConvertTime(input, timeZoneInfo);
                return dateTime;
            }
            catch
            {
                return DateTime.Now;
            }
        }
    }
}
