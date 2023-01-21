﻿using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using MainApp.Services;
using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using MainApp.Components.Chart;

namespace MainApp;

public static class RegisterServices
{
    public static void AddDefaultServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();

        builder.Services.AddServerSideBlazor().AddMicrosoftIdentityConsentHandler();
    }

    public static void AddCachingServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddMemoryCache();
    }

    public static void AddMicrosoftServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();

        // Get Azure AD B2C
        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));

        builder.Services.AddAuthorization(opts =>
        {
            opts.AddPolicy("Admin", policy =>
            {
                policy.RequireClaim("jobTitle", "Admin");
            });
        });
    }

    public static void AddSingletonServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IDbConnection, DbConnection>();

        builder.Services.AddSingleton<IUserData, MongoUserData>();
    }

    public static void AddScopedServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<SpinnerService>();
        builder.Services.AddScoped<ToastService>();
        builder.Services.AddScoped<IChartService, ChartService>();
    }
}