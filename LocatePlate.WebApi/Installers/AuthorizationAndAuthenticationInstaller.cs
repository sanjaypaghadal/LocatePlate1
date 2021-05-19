using LocatePlate.Infrastructure.Constant;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
//using IdentityServer4.AccessTokenValidation;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.IdentityModel.Tokens;

namespace LocatePlate.WebApi.Installers
{
    public class AuthorizationAndAuthenticationInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            #region AuthorizationAndAuthentication

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;//true;
                options.Password.RequireLowercase = false;//true;
                options.Password.RequireNonAlphanumeric = false;//true;
                options.Password.RequireUppercase = false;//true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;//1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(300);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(300);
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/Logout";
                //options.LogoutPath = "home";
                options.SlidingExpiration = true;
            });

            services.AddAuthentication().AddCookie(UserRoles.WebsiteUser, options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
            });
            //.AddCookie(UserRoles.ResaurantOwner, options =>
            //{
            //    options.LoginPath = "/Identity/Account/Login/";
            //    options.LogoutPath = "/Identity/Account/Logout/";
            //}).AddCookie(UserRoles.LocationAdmin, options =>
            //{
            //    options.LoginPath = "/Identity/Account/Login/";
            //    options.LogoutPath = "/Identity/Account/Logout/";
            //});


            services.AddAuthorization(auth =>
            {
                auth.AddPolicy(UserRoles.ResaurantOwner,
                 authBuilder =>
                 {
                     authBuilder.RequireRole(UserRoles.ResaurantOwner);
                 });

                auth.AddPolicy(UserRoles.LocationAdmin,
                  authBuilder =>
                        {
                            authBuilder.RequireRole(UserRoles.LocationAdmin);
                        });

                auth.AddPolicy(UserRoles.WebsiteUser,
                authBuilder =>
                {
                    authBuilder.RequireRole(UserRoles.WebsiteUser);
                });
            });








            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //    .AddJwtBearer(options =>
            //    {
            //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Authentication:SecurityKey")));
            //        options.TokenValidationParameters = new TokenValidationParameters()
            //        {
            //            IssuerSigningKey = key,
            //            ValidAudience = "Audience",
            //            ValidIssuer = "Issuer",
            //            ValidateIssuerSigningKey = true,
            //            ValidateLifetime = true,
            //            ClockSkew = TimeSpan.FromMinutes(0)
            //        };
            //    });
            #endregion
        }
    }
}
