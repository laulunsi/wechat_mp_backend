using System;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Wechat.Backend.Areas.ServiceAccount.Filters;
using Wechat.Backend.Areas.ServiceAccount.Models;
using Wechat.Backend.Extensions;

namespace Wechat.Backend.Areas.ServiceAccount.Controllers
{
    [Route("serviceaccount/[controller]")]
    [ApiController]
    public class SftController : ControllerBase
    {
        private readonly ILog _logger;
        private readonly string _token;

        public SftController(IConfiguration configuration)
        {
            _logger = LogManager.GetLogger(typeof(LogRequestFilter));

            var tokenSection = configuration.GetSection("Weixin:ServiceAccount:Sft:Token");
            if (tokenSection == null)
                throw new NullReferenceException("Weixin:ServiceAccount:Sft:Token in appsettings is null");

            _token = tokenSection.Value;
        }

        /// <summary>
        /// 微信测试连通性
        ///     GET: serviceaccount/sft
        /// </summary>
        /// <param name="nonce"></param>
        /// <param name="echostr"></param>
        /// <param name="signature">签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <returns>param: "echostr"</returns>
        [HttpGet]
        [ServiceFilter(typeof(LogRequestFilter))]
        public string Get(string signature, string timestamp, string nonce, string echostr)
        {
            if (string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(timestamp) || string.IsNullOrEmpty(nonce))
            {
                _logger.Debug($"arguments in qs are invalid, {HttpContext.Request.QueryString}");
                throw new ArgumentNullException("arguments in qs are invalid");
            }

            var array = new[] { _token, timestamp, nonce };
            Array.Sort(array);
            var encrypted = string.Join("", array).Sha1();

            if (string.Compare(signature, encrypted, StringComparison.OrdinalIgnoreCase) != 0)
            {
                _logger.Debug($"signature verification failed, {signature} <> {encrypted}");
                throw new ArgumentNullException("signature verification failed");
            }

            return echostr;
        }

        /// <summary>
        /// 微信推送
        ///     POST: serviceaccount/sft
        /// </summary>
        [HttpPost]
        [ServiceFilter(typeof(LogRequestFilter))]
        public string Post(WechatMessageBase message)
        {
            var response = message.GetReponse();

            return response;
        }
    }
}