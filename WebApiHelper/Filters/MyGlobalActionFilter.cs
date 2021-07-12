using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using WebApiHelper.Models;

namespace WebApiHelper.Filters
{
    /// <summary>
    /// 全局ActionFilter, 继承接口IActionFilter，在  services.AddControllers中注入
    /// 执行顺序
    /// 1，控制器实例化构造函数执行
    /// 2，全局 Filter -OnActionExecuting - 方法执行
    /// 3，控制器Filter - OnActionExecuting - 方法执行
    /// 4，Action Filter - OnActionExecuting - 方法执行
    /// 5，Action逻辑代码执行
    /// 6，Action Filter - OnActionExecuted - 方法执行
    /// 7，控制器 Filter -OnActionExecuted - 方法执行
    /// 8，全局 Filter -OnActionExecuted - 方法执行
    /// </summary>
    public class MyGlobalActionFilter : IActionFilter
    {
        /// <summary>
        /// NLog中间件
        /// </summary>
        private readonly ILogger<MyGlobalActionFilter> _logger;


        public MyGlobalActionFilter(ILogger<MyGlobalActionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// action执行前
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //记录日志
            //filterContext.ActionArguments 请求参，Dictionary字典类型 key是方法的参数名 value是参数值
            //filterContext.ActionDescriptor.Parameters获取方法参数信息 名称等
            Rlog rlog = new Rlog()
            {
                RequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                ActionName = filterContext.ActionDescriptor.DisplayName,//包含了控制器名称和方法名称
                RequestParms = JsonConvert.SerializeObject(filterContext.ActionArguments[filterContext.ActionDescriptor.Parameters[0].Name]),
            };

            //filterContext.HttpContext.Items 可以存一些自定义数据 以便在其他方法 如 OnResultExecuted中使用
            filterContext.HttpContext.Items.Add("Rlog", rlog);

        }


        /// <summary>
        /// action执行后
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Rlog rlog = filterContext.HttpContext.Items["Rlog"] as Rlog;
            rlog.ResponseTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            rlog.ResponseResult = Newtonsoft.Json.JsonConvert.SerializeObject(filterContext.Result);
            _logger.LogInformation(Newtonsoft.Json.JsonConvert.SerializeObject(rlog));
        }


    }
}
