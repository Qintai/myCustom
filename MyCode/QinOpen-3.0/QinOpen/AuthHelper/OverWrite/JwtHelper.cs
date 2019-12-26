using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using QinCommon;
using QinCommon.Common;
using QinCommon.Common.AppConfig;
using QinOpen.AuthHelper;

namespace QinOpen
{
    public class JwtHelper
    {

        /// <summary>
        /// 颁发JWT字符串
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public static string IssueJwt(TokenModelJwt tokenModel)
        {
            /*1.写好这个证件中，有哪些信息，一个Claim 对象代表着一个证件中某一个信息*/
            Claim[] claims = new[]
            {
               new Claim(JwtRegisteredClaimNames.UniqueName, tokenModel.Name), //证件用户名
               new Claim(JwtRegisteredClaimNames.Sid, tokenModel.Uid.ToString()),//  证件Id   
               new Claim(ClaimTypes.Role, tokenModel.Role) //证件的角色，以后控制器上就可以直接这样写 [Authorize(Roles = "test")]
            };

            // 2. 准备 安全密钥。就是： JwtParameter.IssuerSigningKey

            //3.准备 数字签名的安全密钥、算法和摘要。
            SigningCredentials creds = new SigningCredentials(JwtParameter.IssuerSigningKey, SecurityAlgorithms.HmacSha256);

            //4.实例化JWT得到token，
            JwtSecurityToken jst = new JwtSecurityToken(
                issuer: JwtParameter.ValidIssuer,
                audience: JwtParameter.ValidIssuer,  //必须加上这个才能通过验证
                claims: claims,
                expires: DateTime.Now.AddSeconds(60), //token的到期时间，暂时设置的60秒
                signingCredentials: creds
            );

            // 5. 以精简序列化格式将JWT安全令牌序列化为WT。拿到最终的token
            string jwtStr = new JwtSecurityTokenHandler().WriteToken(jst);
            return jwtStr;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static TokenModelJwt SerializeJwt(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
            object Name;
            object Uid;
            object role;
            try
            {
                jwtToken.Payload.TryGetValue(JwtRegisteredClaimNames.UniqueName, out Name);
                jwtToken.Payload.TryGetValue(JwtRegisteredClaimNames.Sid, out Uid);
                jwtToken.Payload.TryGetValue(ClaimTypes.Role, out role);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
            var tm = new TokenModelJwt
            {
                Name= Name.ToString(),
                Uid = Uid.ObjToInt(),
                Role = role != null ? role.ObjToString() : "",
            };
            return tm;
        }
    }

    /// <summary>
    /// 令牌
    /// </summary>
    public class TokenModelJwt
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long Uid { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }

    }
}
