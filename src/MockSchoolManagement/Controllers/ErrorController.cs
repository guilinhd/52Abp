using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockSchoolManagement.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> logger;

        ///<summary>
        ///注入ASP.NET  Core ILogger服务。
        ///将控制器类型指定为泛型参数。
        ///这有助于我们进行确定哪个类或控制器产生了异常，然后记录它
        ///</summary>
        ///<param name="logger"></param>
        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "抱歉，你访问的页面不存在";
                    logger.LogWarning($"发生了一个404错误. 路径 = " +
                            $"{statusCodeResult.OriginalPath} 以及查询字符串 = " +
                            $"{statusCodeResult.OriginalQueryString}");
                    break;
                default:
                    break;
            }
            return View("NoFound");
        }

        [Route("Error")]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            ViewBag.Path = exceptionHandlerPathFeature.Path;
            ViewBag.ErrorMessage = exceptionHandlerPathFeature.Error.Message;
            ViewBag.StackTrace = exceptionHandlerPathFeature.Error.StackTrace;

            return View("Error");
        }
    }
}
