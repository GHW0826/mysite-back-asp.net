using mysite_back_asp.net;
using Water.Common.AspNetCore.Extensions;
using Infrastructure;
using Application;

var builder = WebApplication.CreateBuilder(args);

// water
builder.Host.AddWaterHostBuilderExtension<AppConfig>((options, config) =>
{
    // options.AddApplicationInsightsInitializer<AppInsightsInitializer>();
    // options.AddApplicationInsightsFilter<AppInsightsFilter>();
    // options.AddExceptionFilter<ApiExceptionFilterAttribute>();
});
builder.Services.AddWaterServiceCollectionExtenstion();

var appconfig = builder.Configuration.GetSection("AppConfig").Get<AppConfig>();

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

app.UseWaterApi();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();
// app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// controller mapping
app.MapControllers();

app.Run();
