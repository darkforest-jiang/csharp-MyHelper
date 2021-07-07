using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

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
            base.OnActionExecuting(filterContext);
            //todo

            //请求参数都是一致的 可以获得其中的授权信息 如果不对拒绝执行
            //如果api接口是统一的参数 参数权限验证信息也在里边 在这儿也可以直接获取参数信息做权限验证
            //string token = string.Empty;
            //if(filterContext.HttpContext.Request.Headers.ContainsKey("token"))
            //{
            //    token = filterContext.HttpContext.Request.Headers["token"].ToString();
            //}
            //if (token != "haha")    //验证token
            //{
            //    //返回一个内容result     然后程序就不会进入controller中执行api方法
            //    filterContext.Result = new ContentResult() { 
            //        StatusCode = 404,
            //        ContentType = "拒绝访问",
            //        Content = "无权限访问"
            //    };
            //    return;
            //}

            //记录日志
            //filterContext.ActionArguments 请求参，Dictionary字典类型 key是方法的参数名 value是参数值
            //filterContext.ActionDescriptor.Parameters获取方法参数信息 名称等
            FilterLog flog = new FilterLog()
            {
                ActionName = filterContext.ActionDescriptor.DisplayName,//包含了控制器名称和方法名称
                RequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                RequestParm = JsonConvert.SerializeObject(filterContext.ActionArguments[filterContext.ActionDescriptor.Parameters[0].Name])
            };

            //filterContext.HttpContext.Items 可以存一些自定义数据 以便在其他方法 如 OnResultExecuted中国使用
            filterContext.HttpContext.Items.Add("FilterLog", flog);

        }


        /// <summary>
        /// action执行后
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            //todo
        }

        /// <summary>
        /// action return 结果之前
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            //todo
        }

        /// <summary>
        /// action return 结果之后
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            //todo

            FilterLog flog = filterContext.HttpContext.Items["FilterLog"] as FilterLog;
            Console.WriteLine(JsonConvert.SerializeObject(flog));
            
        }

    }
}
