
using LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.Swagger;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.StartupExtensions
{
    public static class ApiVersioningExtensions
    {
        public static IServiceCollection AddSwaggerApiVersioning(this IServiceCollection services, Assembly assembly)
        {
            services.AddApiVersioning(
                 options =>
                 {
                     // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                     options.ReportApiVersions = true;
                     options.DefaultApiVersion = new(1, 0);
                     options.AssumeDefaultVersionWhenUnspecified = true;
                 });
            services.AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";
                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(
                options =>
                {
                    OpenApiSecurityScheme securityScheme = new()
                    {
                        Name = "JWT Authentication",
                        Description = "Enter JWT Bearer token **_only_**",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer", // must be lower case
                        BearerFormat = "JWT",
                        Reference = new()
                        {
                            Id = JwtBearerDefaults.AuthenticationScheme,
                            Type = ReferenceType.SecurityScheme
                        }
                    };

                    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

                    options.AddSecurityRequirement(new()
                    {
                        { securityScheme, System.Array.Empty<string>() }
                    });

                    // add a custom operation filter which sets default values
                    options.OperationFilter<SwaggerDefaultValues>();

                    // integrate xml commentsi
                    options.IncludeXmlComments(GetXmlCommentsFilePath(assembly));
                });

            return services;
        }

        public static IApplicationBuilder UseSwaggerApiVersioning(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swagger, httpReq) =>

                {
                    swagger.Servers = new List<OpenApiServer> { new() { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" } };
                });
            });

            app.UseSwaggerUI(
            options =>
            {
                // build a swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }

                options.DisplayOperationId();
                options.DisplayRequestDuration();
            });

            return app;
        }

        static string GetXmlCommentsFilePath(Assembly assembly)
        {
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            var fileName = assembly.GetName().Name + ".xml";
            return Path.Combine(basePath, fileName);
        }
    }
}
