using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
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

            services.AddDbContext<TestXmlDbContext>(o
                => o.UseMySql("server=localhost;user id=root;database=test_xml; user=root; password=apollinier13"));
            // => o.UseMySql(Configuration.GetConnectionString("DataBaseConnectionString")));
            services.AddScoped<IUserInfoService, UserInfoService>();
            services.AddScoped<TestXmlDbContext>();
            services.AddControllers();
            
            //AddConfiguration(services);
            AddServiceOptions<AppOptions>(services, "TestXmlOptions");
            
            
            //local cash ()could be change in redis without additional modification in controller
            services.AddDistributedMemoryCache(options => options.ExpirationScanFrequency = TimeSpan.FromMinutes(3));
            services.RegisterSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.RegisterSwaggerUi();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "defaultArea",
                    pattern: "{area:exists}/{controller}/{action}");
            });
        }


        private static void AddConfiguration(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var result = builder.Build();
            services.AddSingleton<IConfiguration>(result);
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
