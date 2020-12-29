using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Support_Your_Locals.Cryptography;
using Support_Your_Locals.Infrastructure;
using Support_Your_Locals.Models;
using Support_Your_Locals.Models.Repositories;

namespace Support_Your_Locals
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
            services.AddDbContext<ServiceDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))); 
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<HashCalculator>();
            services.AddScoped<ILegacyServiceRepository, LegacyServiceRepository>();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddServerSideBlazor();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/SignIn";
                });
            services.AddSwaggerGen(s => s.SwaggerDoc("v1", new OpenApiInfo { Title = "Support Your Locals", Version = "v1" }));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                System.Diagnostics.Debug.WriteLine("The path: " + context.Request.Path.ToString());
                await next();
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStatusCodePagesWithRedirects("/Home/Error?code={0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("advertisement", "business/{businessId:long}", new {Controller = "Business", action = "Index"});
                endpoints.MapControllerRoute("editAdvertisement", "business/editAdvertisement/{businessId:long}", new {Controller = "Business", action = "EditAdvertisement"});
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/orders/{*catchall}", "/Orders/Index");
                endpoints.MapFallbackToPage("/user/{*catchall}", "/User/Index");
            });
            SeedData.EnsurePopulated(app, new HashCalculator());
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support Your Locals"));
        }
    }
}
