using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog.Web;

namespace WebApiHelper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    //添加AspNetCoreRateLimit的json配置文件
                    .ConfigureAppConfiguration((host, config)=> {
                        config.AddJsonFile("appsettings.RateLimit.json", optional:true, reloadOnChange:true);
                    });
                })                                                         
                //添加NLOG日志中间件
                .ConfigureLogging(logging => {
                    logging.ClearProviders();////移除已经注册的其他日志处理程序
                    logging.SetMinimumLevel(LogLevel.Trace);//设置最小的日志级别
                }
                ).UseNLog();
    }
}
