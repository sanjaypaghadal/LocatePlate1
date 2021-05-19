
using LocatePlate.Infrastructure.Domain;
using LocatePlate.Repository.Bookings;
using LocatePlate.Repository.CanadaCities;
using LocatePlate.Repository.Capactities;
using LocatePlate.Repository.MenuCategories;
using LocatePlate.Repository.Menus;
using LocatePlate.Repository.Modules;
using LocatePlate.Repository.PageLayouts;
using LocatePlate.Repository.Pages;
using LocatePlate.Repository.Repositories.DBConnection;
using LocatePlate.Repository.Repositories.Discounts;
using LocatePlate.Repository.Repositories.Review;
using LocatePlate.Repository.Repositories.Search;
using LocatePlate.Repository.Restaurants;
using LocatePlate.Repository.Sections;
using LocatePlate.Repository.Sql.Repositories.Users;
using LocatePlate.Repository.Timings;
using LocatePlate.Service.Bookings;
using LocatePlate.Service.CanadaCities;
using LocatePlate.Service.Capactities;
using LocatePlate.Service.MenuCategories;
using LocatePlate.Service.Menus;
using LocatePlate.Service.Pages;
using LocatePlate.Service.PagesLayouts;
using LocatePlate.Service.Restaurants;
using LocatePlate.Service.Sections;
using LocatePlate.Service.Services.Discounts;
using LocatePlate.Service.Services.Modules;
using LocatePlate.Service.Services.Review;
using LocatePlate.Service.Services.SearchManager;
using LocatePlate.Service.Services.Users;
using LocatePlate.Service.Timings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Http;


namespace LocatePlate.WebApi.DependencyResolution
{
    public static class ServiceCollectionExtension
    {

        private static TimeZoneInfo SetUserTimeZone(string cookieValueFromContext)
        {
            string jsNumberOfMinutesOffset = cookieValueFromContext;   // sending the above offset
            var timeZones = TimeZoneInfo.GetSystemTimeZones();
            var numberOfMinutes = Int32.Parse(jsNumberOfMinutesOffset) * (-1);
            var timeSpan = TimeSpan.FromMinutes(numberOfMinutes);
            var userTimeZone = timeZones.Where(tz => tz.BaseUtcOffset == timeSpan).FirstOrDefault();
            return userTimeZone;
        }

        public static IServiceCollection RegisterDependencies(this IServiceCollection services)
        {
            services.AddSingleton<HttpClient>();

            // register repositories
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<ICapacityRepository, CapacityRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IRestaurantRepository, RestaurantRepository>();
            services.AddScoped<ITimingRepository, TimingRepository>();
            services.AddScoped<ICanadaCityRepository, CanadaCityRepository>();
            services.AddScoped<IMenuCategoryRepository, MenuCatergoryRepository>();
            services.AddScoped<IBookingXMenuRepository, BookingXMenuRepository>();
            services.AddScoped<IBookingXCapacityRepository, BookingXCapacityRepository>();
            services.AddScoped<IPageRepositoryMongoDB, PageRepositoryMongoDB>();
            services.AddScoped<IPageLayoutRepositoryMongoDB, PageLayoutRepositoryMongoDB>();
            services.AddScoped<ISectionRepositoryMongoDB, SectionRepositoryMongoDB>();
            services.AddScoped<IModuleRepositoryMongoDB, ModuleRepositoryMongoDB>();
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddScoped<ISearchManager, SearchManager>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // register services
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<ICapacityService, CapacityService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<ITimingService, TimingService>();
            services.AddScoped<ICanadaCityService, CanadaCityService>();
            services.AddScoped<IMenuCategoryService, MenuCategoryService>();
            services.AddScoped<IBookingXMenuService, BookingXMenuService>();
            services.AddScoped<IBookingXCapacityService, BookingXCapacityService>();
            services.AddScoped<IPageService, PageService>();
            services.AddScoped<IPageLayoutService, PageLayoutService>();
            services.AddScoped<ISectionService, SectionService>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IUserService, UserService>();

            //added by binal patel Start
            services.AddScoped<IConnection, RConnection>();
            //services.AddScoped<IConnection, RConnection>();
            //added by binal patel End

            services.AddScoped<IClientSide>(sp =>
            {
                var client = new ClientSide();
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                string cookieValueFromContext = "";
                httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("timezone", out cookieValueFromContext);
                if (!string.IsNullOrEmpty(cookieValueFromContext))
                {
                    client.ClientInfo = SetUserTimeZone(cookieValueFromContext);
                    var dateTimeUnspec = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                    client.ClientTime = TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, client.ClientInfo);
                }
                return client;
            });

            return services;
        }
    }
}
