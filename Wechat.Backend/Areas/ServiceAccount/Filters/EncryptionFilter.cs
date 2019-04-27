using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Wechat.Backend.Areas.ServiceAccount.Models;

namespace Wechat.Backend.Areas.ServiceAccount.Filters
{
    /// <summary>
    /// 记录日志
    /// </summary>
    public class EncryptionFilter : Attribute, IActionFilter
    {
        private readonly WechatDbContext _context;

        public EncryptionFilter(WechatDbContext context)
        {
            _context = context;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}