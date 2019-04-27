using System;
using Wechat.Backend.Extensions;

namespace Wechat.Backend.Areas.ServiceAccount.Models
{
    public abstract class WechatMessageBase
    {
        public string ToUserName { get; set; }

        public string FromUserName { get; set; }

        public string CreateTime { get; set; }

        public string MsgType { get; set; }

        public virtual string GetReponse()
        {
            var response =
                $"<xml><ToUserName><![CDATA[{FromUserName}]]></ToUserName><FromUserName><![CDATA[{ToUserName}]]></FromUserName>" +
                $"<CreateTime>{DateTime.Now.GetTimestamp()}</CreateTime>" +
                $"<MsgType><![CDATA[{GetResponseMessageType()}]]></MsgType>" +
                GetResponseContent() +
                "</xml>";

            return response;
        }

        protected virtual string GetResponseContent()
        {
            return string.Empty;
        }

        protected virtual string GetResponseMessageType()
        {
            return "text";
        }
    }
}