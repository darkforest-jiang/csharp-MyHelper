using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebApiHelper.Filters
{
    /// <summary>
    /// Filter
    /// 执行顺序OnActionExecuting>OnActionExecuted>OnResultExecuting>Action>OnResultExecuted
    /// </summary>
    public class MyFilter : ActionFilterAttribute
    {
        /// <summary>
        /// action执行前
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            FilterLog flog = new FilterLog()
            {
                ActionName = filterContext.ActionDescriptor.DisplayName,
                RequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                RequestParm = JsonConvert.SerializeObject(filterContext.ActionArguments[filterContext.ActionDescriptor.Parameters[0].Name])
            };
            filterContext.HttpContext.Items.Add("FilterLog", flog);
            base.OnActionExecuting(filterContext);
        }


        /// <summary>
        /// action执行后
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        /// action执行结果之前
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }

        /// <summary>
        /// action执行结果之后
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            FilterLog flog = filterContext.HttpContext.Items["FilterLog"] as FilterLog;
            Console.WriteLine(JsonConvert.SerializeObject(flog));
            base.OnResultExecuted(filterContext);
        }

    }
}
