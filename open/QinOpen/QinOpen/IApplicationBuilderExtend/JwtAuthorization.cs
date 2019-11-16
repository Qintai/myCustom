using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace QinOpen.IApplicationBuilderExtend
{
    public static class JwtAuthorization
    {
        public static void Jwt(this IServiceCollection services, IConfiguration _configuration)
        {
            //读取配置文件
            var symmetricKeyAsBase64 ="FJKLSFJEWPCDSNVKREWPOIDSKLNCKSJFOEWPNVDSLKJFEWOREWPFSDKNALERPOEWRIENDSKNVDSKLJFPOREWPIGJDKSLNARE";
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var Issuer = "RIPOEWIKM,MFMDSM";
            var Audience = "FKLKNKCDEFNALKEFEF";


            services.AddAuthorization(k =>
            {
                k.AddPolicy("EveoneAdmin", p => p.RequireRole("admin_a", "admin_b", "admin_c").Build());
            });




            services.AddAuthentication(f =>
            {
                f.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; //默认认证的方案
                                                                          //   f.DefaultChallengeScheme = nameof(ApiResponseHandler); //  
                                                                          //   f.DefaultForbidScheme = nameof(ApiResponseHandler);  //认证失败的方案
            }).AddJwtBearer(jwt =>
            {
                jwt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = true,
                    ValidIssuer = Issuer,//发行人
                    ValidateAudience = true,
                    ValidAudience = Audience,//订阅人
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30),
                    RequireExpirationTime = true,
                };
                jwt.Events = new JwtBearerEvents() { OnForbidden = { } };
            });

        }
    }
}
