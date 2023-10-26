using mysite_back_asp.net;
using Water.Common.AspNetCore.Extensions;
using Infrastructure;
using Application;
using mysite_back_asp.net.SIgnalR.hub;
using Microsoft.AspNetCore.HttpLogging;

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