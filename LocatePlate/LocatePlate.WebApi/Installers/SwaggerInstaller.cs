using LocatePlate.WebApi.Swagger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace LocatePlate.WebApi.Installers
{
    public class SwaggerInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "LocatePlate",
                    Version = "v1",
                    Description = "The rest api interface for LocatePlate",
                    TermsOfService = new Uri("https://www.locateplate.com/terms-and-conditions"),
                    Contact = new OpenApiContact { Name = "LocatePlate Team", Email = "Support@locateplate.com" },
                    License = new OpenApiLicense { Name = "Use under LocatePlate", Url = new Uri("http://www.LocatePlate.com/privacy-policy") }
                });

                options.DocInclusionPredicate((docName, api) => 
                   api.RelativePath.Contains(docName));

                options.OperationFilter<ClientZoneInfodHeaderParameter>();

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Provide the refresh token with the Bearer keyword e.g. Bearer <token>.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("/connect/authorize", UriKind.Relative),
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid", "openid" },
                                { "role", "role" },
                                { "profile", "profile" },
                                { "api", "api" }
                            }
                        }
                    }
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference=new OpenApiReference {Id="Bearer",Type=ReferenceType.SecurityScheme}
                        },
                        new List<string>()}
                });
            });
        }
    }
}
