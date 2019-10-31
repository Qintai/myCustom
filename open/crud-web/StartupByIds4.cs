using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using crud_base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace crud_web
{
    public class StartupByIds4
    {
        public StartupByIds4(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public IConfiguration configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            /*========IdentityServer4===============================================================================================*/
            //����Tokenǩ����Ҫһ�Թ�Կ��˽Կ������IdentityServerΪ�������ṩ��һ��AddDeveloperSigningCredential()��������������Ǹ㶨����£���Ĭ�ϴ浽Ӳ���С����л�����������ʱ�����ǵ�ʹ�������˾���֤�飬����Ϊʹ��AddSigningCredential()������
            InMemoryConfiguration.Configuration = configuration;

            // var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            var basePath1 = AppContext.BaseDirectory;
            // string basePath2 = Path.GetDirectoryName(typeof(Program).Assembly.Location);

            services.AddIdentityServer()
                // .AddDeveloperSigningCredential()
                .AddSigningCredential(new X509Certificate2(Path.Combine(basePath1,
                        configuration["Certificates:CerPath"]), //֤��λ��
                    configuration["Certificates:Password"])) //֤������
                .AddTestUsers(InMemoryConfiguration.GetUsers().ToList())
                .AddInMemoryClients(InMemoryConfiguration.GetClients())
                .AddInMemoryApiResources(InMemoryConfiguration.GetApiResources());
            /*========IdentityServer4=================================================================================================*/

            services.InjectionBusiness(configuration); //�������ݿ� ������
            services.InjectionBusinessServer(); //����ע��ҵ��
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            /*========IdentityServer4========*/
            // NuGet��IdentityServer4����һ�汾��������EF����˵�˻�ȡtoken��Ȼ���и�IdentityServer4��ҳ�棬
            // https://www.cnblogs.com/edisonchou/p/identityserver4_foundation_and_quickstart_01.html
            // https://localhost:5001/Home/Index
            // https://localhost:5001/Account/Logout �˳�
            // https://localhost:5001/Account/Login ��½

            app.UseIdentityServer(); //��IdentityServer4 ���ӵ��ܵ���
            app.UseStaticFiles(); //Ϊ��ǰ����·�����þ�̬�ļ�����
            app.UseMvcWithDefaultRoute(); //����Ĭ��·��
            /*========IdentityServer4========*/

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            //app.UseMvc();
        }
    }
}