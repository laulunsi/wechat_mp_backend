using System;
using log4net;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Wechat.Backend.Areas.ServiceAccount.Models;

namespace Wechat.Backend.Areas.ServiceAccount.Filters
{
    /// <summary>
    /// 记录日志
    /// </summary>
    public class LogRequestFilter : Attribute, IActionFilter
    {
        private readonly ILog _logger;
        private readonly WechatDbContext _context;

        public LogRequestFilter(WechatDbContext context)
        {
            _context = context;
            _logger = LogManager.GetLogger(typeof(LogRequestFilter));
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.Info(context.HttpContext.Request.Path);

            if (context.HttpContext.Request.Method == "POST")
            {
                // enable to read httpContext.Request.Body
                context.HttpContext.Request.EnableRewind();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var request = context.HttpContext.Request;

            var requestBody = string.Empty;
            if (request.Method == "POST")
            {
                var contentLength = (int) request.ContentLength.GetValueOrDefault();
                var body = new byte[contentLength];
                request.Body.Position = 0;
                request.Body.Read(body, 0, contentLength);
                requestBody = System.Text.Encoding.Default.GetString(body);
            }

            var objectResult = context.Result as ObjectResult;
            var response = objectResult?.Value.ToString();

            var log = new Log
            {
                Method = request.Method,
                RequestPath = request.Path.ToString(),
                RequestQueryString = request.QueryString.ToString(),
                RequestBody = requestBody,
                Response = response,
                CreationTime = DateTime.Now
            };

            _context.Logs.Add(log);
            _context.SaveChanges();
        }
    }
}