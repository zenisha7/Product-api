using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Data;
using Product.Services;


namespace Product
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

        }
        public IConfiguration Configuration { get; }
        private IHostingEnvironment Environment;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddControllersWithViews();
            services.AddDbContext<ProductDbContext>(options =>
            {
                var cs = Configuration.GetConnectionString("ProductDBConnection");
                options.UseSqlServer(cs);
            });

            services.AddScoped<IStockService, StockServices>();
            services.AddScoped<ICustomerService, CustomerServices>();
            // Configure your policies
            services.AddAuthorization(options =>
                  options.AddPolicy("Staff",
                  policy => policy.RequireClaim("role", "Staff", "Admin")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseRouting();

            app.UseEndpoints(routes =>
            {
                routes.MapControllerRoute("default","{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}