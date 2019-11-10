
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace open.Api
{
    public class AjaxResult
    {
        public bool isok { get; set; }

        public string msg { get; set; }
        public string code { get; set; }
        public object data { get; set; }
        public string StatusCode { get; set; }
    }
}
