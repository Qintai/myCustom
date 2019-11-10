/*
*  创建人：喔是传奇 
*  Date: 2019-10-29 09:04:51
*/
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace open.Api
{
    /// <summary>
    /// 自定义的handler
    /// 通常会提供一个统一的认证中心，负责证书的颁发及销毁（登入和登出），而其它服务只用来验证证书，并用不到SingIn/SingOut。
    /// </summary>
    public class MyHandler : IAuthenticationHandler, IAuthenticationSignInHandler, IAuthenticationSignOutHandler
    {
        public AuthenticationScheme Scheme { get; private set; }
        protected HttpContext Context { get; private set; }

        public MyHandler()
        {
            
        }

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            Scheme = scheme;
            Context = context;
            return Task.CompletedTask;
        }
         
        /// <summary>
        /// 开始认证
        /// </summary>
        /// <returns></returns>
        public async Task<AuthenticateResult> AuthenticateAsync()
        {
            var cookie = Context.Request.Cookies["myCookie"];
            if (string.IsNullOrEmpty(cookie))
            {
                return AuthenticateResult.NoResult();
            }
            return AuthenticateResult.Success(this.Deserialize(cookie));
        }
         
        /// <summary>
        /// 认证失败
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            Context.Response.Redirect("/login");
            return Task.CompletedTask;
        }
        
         /// <summary>
         /// 没有权限
         /// </summary>
         /// <param name="properties"></param>
         /// <returns></returns>
        public Task ForbidAsync(AuthenticationProperties properties)
        {
            Context.Response.StatusCode = 403;
            return Task.CompletedTask;
        }
        
        /// <summary>
        /// 登录角色
        /// </summary>
        /// <param name="user"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
        {
            var ticket = new AuthenticationTicket(user, properties, Scheme.Name);
            Context.Response.Cookies.Append("myCookie", this.Serialize(ticket));
            return Task.CompletedTask;
        }
         
        /// <summary>
        /// 退出角色
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task SignOutAsync(AuthenticationProperties properties)
        {
            Context.Response.Cookies.Delete("myCookie");
            return Task.CompletedTask;
        }
        
        /// <summary>
        /// 把 string 转换成 AuthenticationTicket
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private AuthenticationTicket Deserialize(string content)
        {
            byte[] byteTicket = System.Text.Encoding.Default.GetBytes(content);
            return TicketSerializer.Default.Deserialize(byteTicket);
        }
         
        /// <summary>
        /// AuthenticationTicket 转换成 string
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        private string Serialize(AuthenticationTicket ticket)
        {
            byte[] byteTicket = TicketSerializer.Default.Serialize(ticket);
            return Encoding.Default.GetString(byteTicket);
        }
    }

    public class TicketDataFormat : SecureDataFormat<AuthenticationTicket>// IDataSerializer<AuthenticationTicket>//
    {
        public TicketDataFormat(IDataProtector protector)
            : base(TicketSerializer.Default, protector)
        {
        }
    }

}


