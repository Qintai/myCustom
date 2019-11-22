using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace QinOpen.Filter
{
    public class MyBadRequestObjectResult: BadRequestObjectResult
    {
        public MyBadRequestObjectResult() 
            :base(  new ModelStateDictionary())
        { 
        
        }

        public override void ExecuteResult(ActionContext context)
        {
            if (!context.ModelState.IsValid)
            {
                string errmsg = "";
                if (!context.ModelState.IsValid)
                {
                    foreach (var item in context.ModelState.Values)
                    {
                        foreach (var error in item.Errors)
                        {
                            errmsg += error.ErrorMessage + "|";
                        }
                    }
                }
                context.HttpContext.Response.WriteAsync("888888888888-"+ errmsg);
            }
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            if (!context.ModelState.IsValid)
            {
                string errmsg = "";
                if (!context.ModelState.IsValid)
                {
                    foreach (var item in context.ModelState.Values)
                    {
                        foreach (var error in item.Errors)
                        {
                            errmsg += error.ErrorMessage + "|";
                        }
                    }
                }
                context.HttpContext.Response.WriteAsync("888888888888-" + errmsg);
            }
            return Task.CompletedTask;
        }


    }
}
