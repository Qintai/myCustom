using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.Configuration;

namespace crud_web
{
    // POST：http://localhost:5000/connect/token?client_id=client.api.service&client_secret=clientsecret&username=admin&password=123456&grant_type=password
    // 直接复制链接不可用，     postman手动填入，                 
    //   client_id                        client.api.service                    类或者对应来源 ClientId
    //    client_secret                 clientsecret                           类或者对应来源  ClientSecrets
    //    username                     admin                                   类或者对应来源  TestUser
    //    password                      123456                                 类或者对应来源  TestUser
    //    grant_type                    password                               

    /// <summary>
    ///     （1）哪些API可以使用这个AuthorizationServer
    ///     （2）哪些Client可以使用这个AuthorizationServer
    ///     （3）哪些User可以被这个AuthrizationServer识别并授权
    /// </summary>
    public class InMemoryConfiguration
    {
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        ///     定义将使用此IdentityServer的API
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource("clientservice", "CAS Client Service")
            };
        }

        /// <summary>
        ///     定义将使用IdentityServer的应用程序
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "client.api.service",
                    ClientSecrets = new[] {new Secret("clientsecret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new[] {"clientservice"}
                }
            };
        }

        /// <summary>
        ///     定义哪些使用将使用此IdentityServer
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TestUser> GetUsers()
        {
            return new[]
            {
                new TestUser
                {
                    SubjectId = "10001",
                    Username = "admin",
                    Password = "123456"
                }
            };
        }
    }
}