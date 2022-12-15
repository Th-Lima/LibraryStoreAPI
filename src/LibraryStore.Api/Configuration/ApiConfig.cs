﻿using Microsoft.AspNetCore.Mvc;

namespace LibraryStore.Api.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection WebApiConfig(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("Development", builder =>
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

                //options.AddDefaultPolicy(builder =>
                //builder
                //.AllowAnyOrigin()
                //.AllowAnyMethod()
                //.AllowAnyHeader()
                //.AllowCredentials());

                options.AddPolicy("Production", builder =>
                builder
                .WithMethods("GET")
                .WithOrigins("http://www.desenvolvedor.io")
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                //.WithHeaders(HeaderNames.ContentType, "x-custom-header")
                .AllowAnyHeader());
            });

            services.AddControllers();

            return services;
        }

        public static IApplicationBuilder UseMvcConfiguration(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

            return app;
        }
    }
}
