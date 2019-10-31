using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crud_web
{
    public class CustomResult : ActionResult, IStatusCodeActionResult, IActionResult
    {
        public int? StatusCode => StatusCodes.Status200OK;


        public string _msg;

        public bool _isok;
        private string _code;
        private object _data;

        public Task ExecuteResultAsync(ActionContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
 
            HttpResponse response = context.HttpContext.Response;
            ResponseContentTypeHelper.ResolveContentTypeAndEncoding("application/json", response.ContentType, "application/json", out string resolvedContentType, out Encoding resolvedContentTypeEncoding);
            response.ContentType = resolvedContentType;
            response.StatusCode = StatusCodes.Status200OK;
            AjaxResult ajaxResult =  new AjaxResult() { isok=_isok , msg=_msg , code=_code, data=_data};
            string result = Newtonsoft.Json.JsonConvert.SerializeObject(ajaxResult);
            return   response.WriteAsync(result, Encoding.UTF8);
        }

        public CustomResult(string msg,bool isok=true,string code="", object data=null) 
        {
            _msg = msg;
            _isok = isok;
            _code = code;
            _data = data;
        }


    }
}
