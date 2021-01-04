using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApiHelper.Filters;
using WebApiHelper.Models;

namespace WebApiHelper.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [MyFilter]
    public class MyApiController : ControllerBase
    {
        private readonly ILogger<MyApiController> _logger;

        public MyApiController(ILogger<MyApiController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public Rr<string> Hello(Rp<string> rp)
        {
            Rr<string> rr = new Rr<string>();
            rr.Code = 1;
            rr.Msg = "Hello";
            rr.Data = "hahah";

            _logger.LogInformation("hhahahh完成了");

            return rr;
        }
    }
}
