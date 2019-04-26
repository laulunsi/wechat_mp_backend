using System;
using System.Security.Cryptography;
using System.Text;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Wechat.Backend.Areas.ServiceAccount.Models;

namespace Wechat.Backend.Areas.ServiceAccount.Filters
{
    public class ServiceAccountFilter : Attribute, IActionFilter
    {
        private readonly string _token;
        private string _signature;
        private string _timestamp;
        private string _nonce;
        private string _echostr;
        private readonly WechatDbContext _context;
        private readonly ILog _logger;

        public ServiceAccountFilter(IConfiguration configuration,
            WechatDbContext context)
        {
            _context = context;
            _logger = LogManager.GetLogger(typeof(ServiceAccountFilter));

            var tokenSection = configuration.GetSection("Weixin:ServiceAccount:Sft:Token");
            if (tokenSection == null)
                throw new NullReferenceException("Weixin:ServiceAccount:Sft:Token in appsettings is null");

            _token = tokenSection.Value;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var query = context.HttpContext.Request.Query;

            _signature = query["signature"];
            _timestamp = query["timestamp"];
            _nonce = query["nonce"];
            _echostr = query["echostr"];

            if (string.IsNullOrEmpty(_signature) || string.IsNullOrEmpty(_timestamp) || string.IsNullOrEmpty(_nonce))
            {
                context.Result = new UnsupportedMediaTypeResult();
                _logger.Info($"data received: {_signature}.{_timestamp}.{_nonce}.{_echostr}");
                return;
            }

            var array = new string[] {_token, _timestamp, _nonce};
            Array.Sort(array);
            var encrypted = string.Join("", array);

            var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(encrypted));

            if (_signature != BitConverter.ToString(hash))
            {
                context.Result = new UnsupportedMediaTypeResult();
                _logger.Info($"data received: {_signature}.{_timestamp}.{_nonce}.{_echostr}");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var log = new Log
            {
                Signature = _signature,
                Timestamp = _timestamp,
                Nonce = _nonce,
                Echostr = _echostr,
                CreationTime = DateTime.Now
            };

            _context.Logs.Add(log);
            _context.SaveChanges();
        }
    }
}