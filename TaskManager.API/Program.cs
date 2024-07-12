using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using TaskManager.API.Common.ResponseModels;
using TaskManager.API.Extensions;
using TaskManager.API.Filters;
using TaskManager.Application;
using TaskManager.Core;
using TaskManager.Infrastructure;
using TaskManager.Shared;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddControllers(options =>
    {
        options.Filters.Add(new ApiExceptionFilter());
    })
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.Converters.Add(new TrimJsonStringFilter());
    });

builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
    // Custom response from ModelValidation
    opt.InvalidModelStateResponseFactory = actionContext =>
    {
        var errors = actionContext.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .ToDictionary(a => a.Key, a => a.Value?.Errors.Select(b => b.ErrorMessage).ToArray());
        
        var response = new BadRequestObjectResult(new BadRequestResponse
        {
            Title = "One or more validation failures have occurred",
            StatusCode = 400,
            Errors = errors
        });

        return response;
    };
});

// builder.Host.UseSerilog((context, configuration) =>
//     configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDistributedMemoryCache(); // Dodaj cache pamięciowy (in-memory)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Ustaw czas wygaśnięcia sesji
    options.Cookie.HttpOnly = true; // Ustawienia ciasteczek
    options.Cookie.IsEssential = true; // Ciasteczka sesji są niezbędne
});
 
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

builder.Services.AddCorsPolicy(builder.Configuration);
builder.Services.AddIdentityConfig(builder.Configuration);
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfig();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddApplication();
builder.Services.AddCore();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

Globals.ApplicationUrl = builder.Configuration.GetValue<string>("ApplicationConfig:ApplicationUrl");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseSerilogRequestLogging();

app.Use(async (context, next) =>
{
    context.Features.Get<IHttpMaxRequestBodySizeFeature>()!.MaxRequestBodySize = 1000000000;
    await next();
});

app.UseForwardedHeaders(); 
app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("CorsPolicy");
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();