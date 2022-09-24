﻿using LibraryStore.Business.Interfaces;
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

            services.AddScoped<IProviderRepository, ProviderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();

            services.AddScoped<INotifier, Notifier>();

            services.AddScoped<IProviderService, ProviderService>();
            services.AddScoped<IProductService, ProductService>();

            return services;
        }
    }
}