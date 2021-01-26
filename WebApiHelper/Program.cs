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
                    //���AspNetCoreRateLimit��json�����ļ�
                    .ConfigureAppConfiguration((host, config)=> {
                        config.AddJsonFile("appsettings.RateLimit.json", optional:true, reloadOnChange:true);
                    });
                })                                                         
                //���NLOG��־�м��
                .ConfigureLogging(logging => {
                    logging.ClearProviders();////�Ƴ��Ѿ�ע���������־�������
                    logging.SetMinimumLevel(LogLevel.Trace);//������С����־����
                }
                ).UseNLog();
    }
}
