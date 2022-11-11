using LibraryStore.Api.Configuration;
using LibraryStore.Data.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

#region Dependency Injection
builder.Services.ResolveDependencies();
#endregion

var app = builder.Build();

app.UseAuthentication();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthorization();
app.MapControllers();

#region MVC Config
app.UseMvcConfiguration();
#endregion

app.Run();
