using Unit_Testing_xUnit_ASP_NET_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unit_Testing_xUnit_ASP_NET_Core.Utilities
{
    public static class Response
    {
        public static ResponseModel ResponseMessage(string Code, string Status, string Message, object Data)
        {
            ResponseModel Hasil = new ResponseModel();
            Hasil.Code = Code;
            Hasil.Status = Status;
            Hasil.Message = Message;
            Hasil.Data = Data;
            return Hasil;
        }
    }
}
