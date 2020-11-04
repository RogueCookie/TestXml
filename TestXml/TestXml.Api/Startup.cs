using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using TestXml.Abstract;
using TestXml.Abstract.Models.Options;
using TestXml.Api.Extension;
using TestXml.Business;
using TestXml.Data;

namespace TestXml.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddXmlDataContractSerializerFormatters();
            
            services.AddDbContext<TestXmlDbContext>(o
                => o.UseMySql("server=localhost;user id=root;database=test_xml; user=root; password=apollinier13"));
            
            services.AddScoped<IUserInfoService, UserInfoService>();
            services.AddControllers();
            
            //local cash ()could be change in redis without additional modification in controller
            services.AddDistributedMemoryCache(options => options.ExpirationScanFrequency = TimeSpan.FromMinutes(3));
            services.RegisterSwagger();

            // configure basic authentication 
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            
            AddServiceOptions<AppOptions>(services, "TestXmlOptions");

          
            //services.AddMvc()
            //    .AddXmlSerializerFormatters()
            //    .AddXmlDataContractSerializerFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.RegisterSwaggerUi();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseExceptionHandler(env.IsDevelopment() ? "/error": "/error-local-development" /*: "/error"*/);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "defaultArea",
                    pattern: "{area:exists}/{controller}/{action}");
            });
        }

        public static IServiceCollection AddServiceOptions<TOptions>(IServiceCollection services, string sectionName) where TOptions : class
            => services.AddSingleton<TOptions>(sp =>
            {
                var configurations = sp.GetRequiredService<IEnumerable<IConfiguration>>();
                foreach (var configuration in configurations)
                {
                    var section = configuration.GetSection(sectionName);
                    if (!section.Exists()) continue;

                    return section.Get<TOptions>();
                }

                return default;
            });
    }
}
