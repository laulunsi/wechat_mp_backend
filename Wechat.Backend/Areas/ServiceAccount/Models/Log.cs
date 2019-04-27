using System;

namespace Wechat.Backend.Areas.ServiceAccount.Models
{
    public class Log
    {
        public int Id { get; set; }

        public string Method { get; set; }

        public string RequestPath { get; set; }

        public string RequestQueryString { get; set; }

        public string RequestBody { get; set; }

        public string Response { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
