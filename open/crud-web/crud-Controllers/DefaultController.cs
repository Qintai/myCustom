using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
        /// <summary>
        public async Task<object> GetJwt(string name = "", string pass = "")
        {
            string jwtStr = string.Empty;
            bool suc = false;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pass))
            {
                return new JsonResult(new
                {
                    Status = false,
                    message = "用户名或密码不能为空"
                });
            }

            PermissionRequirement _requirement =(PermissionRequirement) HttpContext.RequestServices.GetService(typeof(PermissionRequirement));

            var userRoles = "Admin";
            //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
            var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(JwtRegisteredClaimNames.Jti, "1"), //假设当前的用户ID=1
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds).ToString()) };
            claims.AddRange(userRoles.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));

            //用户标识
            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            identity.AddClaims(claims);
            var token = JwtToken.BuildJwtToken(claims.ToArray(), _requirement);


            return new JsonResult(token);

        }



 

        [Authorize(AuthenticationSchemes = "myAuthentication")] // myAuthentication,自己自定义的认证方案