using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Wechat.Backend.Areas.ServiceAccount.Filters
{
    public class ServiceAccountFilter : Attribute, IActionFilter
    {
        private const string Token = "dKe82lL20_32";

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var query = context.HttpContext.Request.Query;

            var signature = query["signature"];
            var timestamp = query["timestamp"];
            var nonce = query["nonce"];

            if (string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(timestamp) || string.IsNullOrEmpty(nonce))
            {
                context.Result = new UnsupportedMediaTypeResult();
                return;
            }

            var array = new string[] {Token, timestamp, nonce};
            Array.Sort(array);
            var encrypted = string.Join("", array);

            var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(encrypted));

            if (signature != BitConverter.ToString(hash))
            {
                context.Result = new UnsupportedMediaTypeResult();
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}