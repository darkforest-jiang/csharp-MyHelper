using System;
using System.Collections.Generic;

#nullable disable

namespace EFHelper.Models
{
    public partial class TmyUser
    {
        public string UserId { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public int? Enabled { get; set; }
    }
}
