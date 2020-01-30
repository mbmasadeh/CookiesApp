using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using CookiesApp.Data;
using Microsoft.EntityFrameworkCore;

namespace CookiesApp
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
            services.AddAuthentication("CookiesInstance")
                .AddCookie(("CookiesInstance"),options =>
                {
                    options.LoginPath = new PathString("/Account/Login/");
                    options.AccessDeniedPath = new PathString("/Account/AccessDenied");
                    options.ExpireTimeSpan = new TimeSpan(5, 0, 0, 0, 0);
                    options.SlidingExpiration = true;
                });
            services.AddMvc();

            #region Oracle connection
            services.AddDbContext<OracleDbContext>(options =>
            {
                options.UseOracle("Data Source=(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 172.20.10.125)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = testdb01)));User ID=sharepoint;Password=SHAREPOINT#22;");
            });
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default","{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
