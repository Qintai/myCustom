using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace QinOpen.Middleware
{
    public class MyIClientErrorFactory : IClientErrorFactory
    {
        public IActionResult GetClientError(ActionContext actionContext, IClientErrorActionResult clientError)
        {
            return new JsonResult("自定义的错误信息");
        }
    }
}
