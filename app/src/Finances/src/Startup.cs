using Finances.Data;
using Finances.Middlewares.Authentication;
using Finances.Middlewares.Errors;
using Finances.Models;
using Finances.Services.Books;
using Finances.Services.Sessions;
using Finances.Services.Users;
using Finances.Services.Wallets;
using Finances.Web.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace Finances {
    public class Startup {
        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration["Data:DefaultConnection:ConnectionString"]));
            
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddCors(options => {
                options.AddPolicy("AllowSpecificOrigin", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddMvc()
                .AddJsonOptions(options => {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            services.Configure<MvcOptions>(options => {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowSpecificOrigin"));
            });
            
            services.AddTransient<ISessionStore, SessionStore>();
            services.AddTransient<ISessionAccessor, SessionAccessor>();
            services.AddTransient<IAppUserStore, AppUserStore>();
            services.AddTransient<IBookStore, BookStore>();
            services.AddTransient<IWalletStore, WalletStore>();

            services.AddSingleton<DatabaseInititalizer>();
        }

        public void Configure(IApplicationBuilder app, 
                              IHostingEnvironment env, 
                              ILoggerFactory loggerFactory,
                              DatabaseInititalizer databaseInititalizer) {

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseHandleErrorsMiddleware(new HandleErrorsMiddlewareOptions() {
                EnableStackTrace = env.IsDevelopment()
            });

            databaseInititalizer.Initialize().Wait();

            app.UseStaticFiles();
            
            app.UseTokenAuthentication(new TokenAuthenticationOptions() {
                AutomaticAuthenticate = true
            });

            app.UseCors("AllowSpecificOrigin");

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{*path}",
                    defaults: new { controller = "App", action = "Index" });
            });
        }
    }
}
