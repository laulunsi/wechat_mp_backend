using Microsoft.AspNetCore.Mvc;
using Wechat.Backend.Areas.ServiceAccount.Filters;

namespace Wechat.Backend.Controllers
{
    [Route("/form")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class FormController : Controller
    {
        /// <summary>
        /// 用户申请单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Intention")]
        [ServiceFilter(typeof(LogRequestFilter))]
        public IActionResult Intention(int id)
        {
            return View();
        }
    }
}