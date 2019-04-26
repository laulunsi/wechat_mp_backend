using Microsoft.AspNetCore.Mvc;
using Wechat.Backend.Areas.ServiceAccount.Filters;

namespace Wechat.Backend.Areas.ServiceAccount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SftController : ControllerBase
    {
        // GET: api/sft
        [HttpGet]
        [ServiceFilter(typeof(ServiceAccountFilter))]
        public string Get(string echostr)
        {
            return echostr;
        }
    }
}