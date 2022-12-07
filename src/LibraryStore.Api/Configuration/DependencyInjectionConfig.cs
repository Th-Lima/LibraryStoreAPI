using LibraryStore.Api.Extensions;
using LibraryStore.Api.JwtAuthentication;
using LibraryStore.Business.Interfaces;
using LibraryStore.Business.Notifications;
using LibraryStore.Business.Services;
using LibraryStore.Data.Context;
using LibraryStore.Data.Repository;

namespace LibraryStore.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<LibraryStoreDbContext>();

            //Repositories
            services.AddScoped<IProviderRepository, ProviderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();

            //Notification
            services.AddScoped<INotifier, Notifier>();

            //Services
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IProviderService, ProviderService>();
            services.AddScoped<IProductService, ProductService>();

            //JWT auth
            services.AddScoped<IJwtSettings, JwtSettings>();

            //Http AspNet
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();

            return services;
        }
    }
}
