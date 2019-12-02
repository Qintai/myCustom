using Microsoft.IdentityModel.Tokens;
using QinCommon.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QinOpen.AuthHelper
{
    public class JwtParameter
    {
        public static string ValidIssuer
        { 
            get 
            {
                return Appsettings.app(new string[] { "AppSettings", "Tokens", "Issuer" });
            }
        }

        public static SecurityKey IssuerSigningKey
        {
            get
            {
                return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Appsettings.app(new string[] { "AppSettings", "Tokens", "Key" })));
            }
        }


    }
}
