using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiHelper.Filters
{
    public class FilterLog
    {
        public string ActionName { get; set; }

        public string RequestTime { get; set; }

        public string RequestParm { get; set; }

        public string ResponseTime { get; set; }

        public string ResponseResult { get; set; }
    }
}
