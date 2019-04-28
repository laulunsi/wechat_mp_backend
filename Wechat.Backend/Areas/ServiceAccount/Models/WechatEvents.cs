namespace Wechat.Backend.Areas.ServiceAccount.Models
{
    public abstract class WechatEvent : WechatMessageBase
    {
        public WechatEvent()
        {
            MsgType = "event";
        }

        public string Key { get; set; }
    }

    /// <summary>
    /// 关注事件
    /// </summary>
    public class WechatSubscribeEvent : WechatEvent
    {
        protected override string GetResponseMessageType()
        {
            return "news";
        }

        protected override string GetResponseContent()
        {
            // 返回“智能网联出租车”的整体介绍
            var title = "欢迎您加入顺风出租";
            var desc = "号外！号外！！出租车司机朋友们注意啦，我们的顺风出租已经上线，提高咱们出租车司机收入的时候到啦~";
            var picUrl = "http://static.aibol.com/sft/index_small.gif";
            var url = "https://mp.weixin.qq.com/s/6fbVAUkY6mnhEXmEiUWMBw";

            var content = "<ArticleCount>1</ArticleCount>" +
                          $"<Articles><item><Title><![CDATA[{title}]]></Title>" +
                          $"<Description><![CDATA[{desc}]]></Description>" +
                          $"<PicUrl><![CDATA[{picUrl}]]></PicUrl>" +
                          $"<Url><![CDATA[{url}]]></Url></item></Articles>";

            return content;
        }
    }

    /// <summary>
    /// 取消关注事件
    /// </summary>
    public class WechatUnsubscribeEvent : WechatEvent
    {
        public override string GetReponse()
        {
            return string.Empty;
        }
    }
}