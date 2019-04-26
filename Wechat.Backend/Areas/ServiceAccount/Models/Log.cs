using System;

namespace Wechat.Backend.Areas.ServiceAccount.Models
{
    public class Log
    {
        public int Id { get; set; }

        public string Signature { get; set; }

        public string Timestamp { get; set; }

        public string Nonce { get; set; }

        public string Echostr { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
