using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using afc_studio.Models.Entitys;
using CorallJewelry.Entitys;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace CorallJewelry
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

            services.AddControllersWithViews(); 
            services.AddDistributedMemoryCache();
            //services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(100000);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddMvc();
            services.AddDbContextPool<FrontendContext>(
               options => options.UseMySql("Server=localhost;Database=CorallJewelry;User=u0893_adminDB;Password=snRq40~6;",
                   mySqlOptions =>
                   {
                       mySqlOptions.ServerVersion(new Version(5, 6, 45), ServerType.MySql);
                   }
           ));
            services.AddDbContextPool<BackendContext>(
              options => options.UseMySql("Server=localhost;Database=CorallJewelry;User=u0893_adminDB;Password=snRq40~6;",
                  mySqlOptions =>
                  {
                      mySqlOptions.ServerVersion(new Version(5, 6, 45), ServerType.MySql);
                  }
          ));
            services.AddDbContextPool<MainContext>(
              options => options.UseMySql("Server=localhost;Database=u0893839_chat;User=u0893_adminDB;Password=snRq40~6;",
                  mySqlOptions =>
                  {
                      mySqlOptions.ServerVersion(new Version(5, 6, 45), ServerType.MySql);
                  }
          ));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
