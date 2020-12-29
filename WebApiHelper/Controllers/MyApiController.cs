using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost]
        public Rr<string> Hello(Rp<string> rp)
        {
            Rr<string> rr = new Rr<string>();
            rr.Code = 1;
            rr.Msg = "Hello";
            rr.Data = "hahah";
            Thread.Sleep(1 * 1000);
            return rr;
        }
    }
}
