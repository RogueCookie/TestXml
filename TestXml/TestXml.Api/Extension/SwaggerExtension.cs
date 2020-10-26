using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace TestXml.Api.Extension
{
    /// <summary>
    /// Swagger set up. Implementation of Swagger settings
    /// </summary>
    public static class SwaggerExtension
    {
        public static void RegisterSwagger(this IServiceCollection services)
        {
            // Register the Swagger generator
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("testXml", new OpenApiInfo
                {
                    Title = "API for demonstrate what I learn for this task",
                    Description = "Task from the company ...",
                });

                // path from which project to read xml documentation
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                var xmlModulesPath = Path.Combine(AppContext.BaseDirectory, "TestXml.Abstract.xml");

                c.IncludeXmlComments(xmlCommentsFullPath);
                c.IncludeXmlComments(xmlModulesPath);
            });
        }
    }

    /// <summary>
    /// Add swagger in middleware
    /// </summary>
    public static class SwaggerUiExtension
    {
        /// <summary>
        /// Register swagger UI and Handler for exceptions for it
        /// </summary>
        public static void RegisterSwaggerUi(this IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"testXml/swagger.json", $"TestXml");
                c.DisplayRequestDuration();
            });
        }
    }
}