using Autofac;
using BlogProje.Infrastructure;
using Business.DependencyResolvers.Autofac;
using Core.Infrastructure.Abstract;
using Core.Utilities.Security.JWT;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics; // <-- eklendi
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Security; // <-- eklendi (SecurityException i�in)

namespace BlogProje
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;
        public IConfiguration Configuration { get; }

        // Autofac container
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacBusinessModule());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.Configure<TokenOptions>(Configuration.GetSection("TokenOptions"));

            // mevcut DI'lar�n
            services.AddSingleton<Core.Infrastructure.Abstract.IImageStorage, LocalImageStorage>();
            services.AddSingleton<IImageStorage, LocalImageStorage>();

            // COOKIE AUTH
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Admin/Account/Login";
                    options.AccessDeniedPath = "/Admin/Account/AccessDenied";
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromDays(7);
                });
            services.AddHttpContextAccessor();
            Core.Utilities.IoC.ServiceTool.Create(services);
            services.AddAuthorization();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Geli�tirmede detayl� hata sayfas�
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Production: Business'tan gelen yetki hatas�n� AccessDenied'a y�nlendir
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        var feature = context.Features.Get<IExceptionHandlerPathFeature>();
                        var ex = feature?.Error;

                        // Engin'in SecuredOperation'� �o�unlukla SecurityException atar.
                        // Baz� projelerde AuthorizationException gibi �zel tip olabilir.
                        if (ex is SecurityException || ex?.GetType().Name == "AuthorizationException")
                        {
                            context.Response.Redirect("/Admin/Account/AccessDenied");
                            return;
                        }

                        // Di�er hatalarda genel hata sayfas�
                        context.Response.Redirect("/Home/Error");
                    });
                });

                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            // YETK� �ST�SNASI YAKALAYICI (her ortamda)
            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception ex)
                {
                    // SecurityException veya �zel AuthorizationException'� yakala
                    if (ex is System.Security.SecurityException || ex.GetType().Name == "AuthorizationException")
                    {
                        context.Response.Redirect("/Admin/Account/AccessDenied");
                        return;
                    }
                    throw; // di�er hatalar� normal ak��a b�rak
                }
            });
            // S�ra �nemli
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // Areas
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                // Default
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
