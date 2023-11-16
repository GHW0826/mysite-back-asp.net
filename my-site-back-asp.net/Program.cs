using SnowFall.Extensions;

using Application;
using mysite_back_asp.net.SIgnalR.hub;
using Microsoft.AspNetCore.HttpLogging;
using Application.Common;
using Infra;

var builder = WebApplication.CreateBuilder(args);

// water
builder.Host.AddSnowFallApi<AppConfig>((options, config) =>
{
    // options.AddApplicationInsightsInitializer<AppInsightsInitializer>();
    // options.AddApplicationInsightsFilter<AppInsightsFilter>();
    // options.AddExceptionFilter<ApiExceptionFilterAttribute>();
});

var appconfig = builder.Configuration.GetSection("AppConfig").Get<AppConfig>();

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfra(builder.Configuration);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpLogging(logging => {
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("Referer");
    logging.ResponseHeaders.Add("MyResponseHeader");
});

// SignalR
builder.Services.AddSignalR();
var app = builder.Build();

app.UseHttpLogging();

// SignalR
app.MapHub<ChatHub>("/Chat");

app.UseWaterApi();

app.UseExceptionHandler("/Error");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();
// app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// controller mapping
app.MapControllers();

app.Run();