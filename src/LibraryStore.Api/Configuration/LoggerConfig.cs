using LibraryStore.Api.Extensions;

namespace LibraryStore.Api.Configuration
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddElmahIo(o =>
            {
                o.ApiKey = "39044deb28964185b2ba707efe0580d4";
                o.LogId = new Guid("eb6ceaad-fa9d-4501-8873-50875a2a4c6e");
            });

            services.AddHealthChecks()
                .AddElmahIoPublisher(o =>
                {
                    o.ApiKey = "39044deb28964185b2ba707efe0580d4";
                    o.LogId = new Guid("eb6ceaad-fa9d-4501-8873-50875a2a4c6e");
                    o.HeartbeatId = "API LibraryStore";
                })
                .AddCheck("Products", new SqlServerHealthCheck(configuration.GetConnectionString("DefaultConnection")))
                .AddSqlServer(configuration.GetConnectionString("DefaultConnection"), name: "DatabaseSQL");

            services.AddHealthChecksUI()
                .AddSqlServerStorage(configuration.GetConnectionString("DefaultConnection"));

            //services.AddLogging(builder =>
            //{
            //    builder.AddElmahIo(o =>
            //    {
            //        o.ApiKey = "39044deb28964185b2ba707efe0580d4";
            //        o.LogId = new Guid("eb6ceaad-fa9d-4501-8873-50875a2a4c6e");
            //    });
            //    builder.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning);
            //});

            return services;

        }

        public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app)
        {
            app.UseElmahIo();

            return app;
        }
    }
}
