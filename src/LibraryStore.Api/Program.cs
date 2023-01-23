using LibraryStore.Api.Configuration;
using LibraryStore.Api.Extensions;
using LibraryStore.Data.Context;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(Program));

#region Identity
builder.Services.AddIdentityConfiguration(builder.Configuration);
#endregion

builder.Services.AddDbContext<LibraryStoreDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

#region WebApi Config
builder.Services.WebApiConfig();
#endregion

#region Swagger
builder.Services.AddSwaggerConfig();
#endregion

#region Logger
builder.Services.AddLoggingConfiguration();
#endregion

#region Dependency Injection
builder.Services.ResolveDependencies();
#endregion

var app = builder.Build();

app.UseAuthentication();

if (app.Environment.IsDevelopment())
{
    app.UseCors("Development");
}
else
{
    app.UseCors("Production");
    app.UseHsts();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

#region MVC Config
app.UseMvcConfiguration();
#endregion

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwaggerConfig(apiVersionDescriptionProvider, app);

app.UseLoggingConfiguration();

app.Run();
