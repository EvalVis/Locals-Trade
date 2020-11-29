using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RestAPI.Models;
using RestAPI.Models.Repositories;

namespace RestAPI {
    public class Startup
    {

        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ServiceDbContext>(opts => opts.UseSqlServer(Configuration["ConnectionStrings:DatabaseConnection"]));
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddControllers();
            services.Configure<JsonOptions>(opts => opts.JsonSerializerOptions.IgnoreNullValues = true);
            services.AddSwaggerGen(s => s.SwaggerDoc("v1", new OpenApiInfo {Title = "RestAPI for Support Your Locals", Version = "v1"}));
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "RestAPI"));
        }
    }
}
