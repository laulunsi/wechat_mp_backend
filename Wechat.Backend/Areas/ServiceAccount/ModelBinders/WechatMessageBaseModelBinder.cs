using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Wechat.Backend.Areas.ServiceAccount.Models;

namespace Wechat.Backend.Areas.ServiceAccount.ModelBinders
{
    public class WechatMessageBaseModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var request = bindingContext.HttpContext.Request;
            if (request.Method != "POST")
                return Task.CompletedTask;

            request.EnableRewind();

            // 读取http body
            var contentLength = (int) request.ContentLength.GetValueOrDefault();
            var body = new byte[contentLength];
            request.Body.Position = 0;
            request.Body.Read(body, 0, contentLength);
            var requestBody = Encoding.Default.GetString(body);

            // 解析为微信消息
            var model = ResolveWechatMessage(requestBody);
            bindingContext.Result = ModelBindingResult.Success(model);

            return Task.CompletedTask;
        }

        private WechatMessageBase ResolveWechatMessage(string requestBody)
        {
            var msgType = GetSection(requestBody, "MsgType");

            var typeName = string.Empty;

            switch (msgType)
            {
                case "Event":
                    var eventName = GetSection(requestBody, "Event");
                    typeName = $"Wechat{eventName}Event";
                    break;
            }

            if (string.IsNullOrEmpty(typeName))
                return null;

            var type = Assembly
                .Load("Wechat.Backend")
                .GetType($"Wechat.Backend.Areas.ServiceAccount.Models.{typeName}");

            WechatMessageBase message;

            using (var reader = new StringReader(requestBody))
            {
                var serializer = new XmlSerializer(type, new XmlRootAttribute("xml"));
                message = (WechatMessageBase)serializer.Deserialize(reader);
                reader.Close();
            }
            
            return message;
        }

        private static string GetSection(string requestBody, string key)
        {
            var startNode = $"<{key}>";
            var len = startNode.Length;

            var start = requestBody.IndexOf(startNode);
            var end = requestBody.IndexOf($"</{key}>");

            var cdata = requestBody.Substring(start + len, end - start - len);

            var section = cdata.Substring(9).TrimEnd(']', '>').ToLower();

            section = section.Substring(0, 1).ToUpper() + section.Substring(1);

            return section;
        }
    }

    public class WechatMessageBaseEntityBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (context.Metadata.ModelType == typeof(WechatMessageBase))
                return new BinderTypeModelBinder(typeof(WechatMessageBaseModelBinder));

            return null;
        }
    }
}