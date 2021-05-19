using LocatePlate.Infrastructure.Constant;
using LocatePlate.WebApi.Installers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Security.Claims;

namespace LocatePlate.WebApi
{
    public class Startup
    {

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            //  Configuration = configuration;

            var builder = new ConfigurationBuilder()
    .SetBasePath(env.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.Development.json", optional: true)
    .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);
            services.InstallServiceAssemblies(Configuration);
            //services.AddAuthorization(options =>
            //{
            //    var userAuthPolicyBuilder = new AuthorizationPolicyBuilder();
            //    options.DefaultPolicy = userAuthPolicyBuilder
            //                        .RequireAuthenticatedUser()
            //                        .RequireClaim(ClaimTypes.Role)
            //                        .Build();
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
                app.UseDeveloperExceptionPage();
            //    app.UseExceptionHandler("/errorpage");
            //}
            app.UseStatusCodePages();
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/errorpage";
                    await next();
                }
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();


            //  app.UseIdentityServer();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Dashboard}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
                //options.SwaggerEndpoint("/swagger/v2/swagger.json", "V2");
                /// options.RoutePrefix = string.Empty;
                /// 
                options.OAuth2RedirectUrl("https://localhost:44306/");
                options.OAuthScopeSeparator(" ");
                options.OAuthClientId(Configuration.GetValue<string>("IdentityServer:Client:Id"));
                options.OAuthAppName(Configuration.GetValue<string>("IdentityServer:Client:Name"));
                options.OAuthClientSecret(Configuration.GetValue<string>("IdentityServer:Client:Secret"));
            });

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            };
        }
    }
}
