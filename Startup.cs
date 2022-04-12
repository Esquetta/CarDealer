using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarDealer.DataAcces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.IdentityModel.Tokens;
using Rehper.Helpers;
using CarDealer.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CarDealer.AppIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using CarDealer.Resources;
using Microsoft.AspNetCore.Localization.Routing;
using System.Net.Mail;
using CarDealer.Services.Abstract;
using CarDealer.Services.Concrete;
using CarDealer.Provider;
using CarDealer.DataAcces.Abstract;
using CarDealer.DataAcces.Concrete;
using CarDealer.Filter;
using CarDealer.Helpers.Abstract;
using CarDealer.Helpers.Concrete;
using QRCoder;

namespace CarDealer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("appsettings:Token").Value);

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddSingleton<LocService>();
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddScoped<IUserDal, EFUserDal>();
            services.AddScoped<IPhotoDal, EFPhotoDal>();
            services.AddScoped<ICarDal, EFCarDal>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<ISmtpService, SmtpService>();
            services.AddScoped<SmtpClient, SmtpClient>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IActivityLogDal,EFActivityLogDal>();
            services.AddScoped<CarDealerContext, CarDealerContext>();
            services.AddScoped<ISoldCarOwnerDal, SoldCarOwnerDal>();
            services.AddScoped<IQRCoder,CarQrGenerator>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer("Data Source=DESKTOP-EMPIGDT;Initial Catalog=CarGallery;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));
            services.AddIdentity<AppIdentityUser, AppIdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddTokenProvider("Email",typeof(EmailTokenProvider<AppIdentityUser>)).AddDefaultTokenProviders();
            services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
            services.AddDistributedMemoryCache();
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            services.AddSession(options =>
            {
                options.Cookie.Name = "CarDealerCookie";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
            }
            );
            services.AddControllersWithViews(options=>options.Filters.Add(typeof(ActivityLoggerFilter)))
                 .AddNewtonsoftJson(options =>
                     options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                 );



            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportCultures = new List<CultureInfo> {
                new CultureInfo("en-US"),
                new CultureInfo("tr-TR")
                };
                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                options.SupportedCultures = supportCultures;
                options.SupportedUICultures = supportCultures;
                options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
            });

            services.AddAutoMapper(typeof(Startup));


            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;

                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedAccount = true;

            });







        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseAuthorization();
            app.UseAuthentication();
            app.UseSession();
            app.UseRouting();
            
            app.UseMvc(ConfigureRoutes);


        }

        private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("Default", "{controller=Car}/{action=Index}/{id?}");
            routeBuilder.MapRoute("Login", "{controller=Account}/{action=Login}");
            routeBuilder.MapRoute("AccesDenied", "Login/{controller=Management}/{action=Login}");

        }
    }
}
