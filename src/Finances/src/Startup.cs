using Finances.Data;
using Finances.Middlewares.Authentication;
using Finances.Models;
using Finances.Services.Sessions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
                options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc()
                .AddJsonOptions(options => {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            services.AddTransient<ISessionStore, SessionStore>();

            services.AddSingleton<DatabaseInititalizer>();
        }

        public void Configure(IApplicationBuilder app, 
                              IHostingEnvironment env, 
                              ILoggerFactory loggerFactory,
                              DatabaseInititalizer databaseInititalizer) {

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
            }

            databaseInititalizer.Initialize().Wait();

            app.UseStaticFiles();
            
            app.UseTokenAuthentication(new TokenAuthenticationOptions() {
                AutomaticAuthenticate = true
            });

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{*path}",
                    defaults: new { controller = "App", action = "Index" });
            });
        }
    }
}
