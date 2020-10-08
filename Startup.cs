using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestZL.DAL;
using TestZL.Helpers;
using TestZL.Services;
using TestZL.Services.Interfaces;

namespace TestZL
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
            string connection = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<TestZLContext>(options =>
                options.UseSqlServer(connection));
            services.AddTransient<IDictService, DictService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<ILoadItemService, LoadItemService>();
            services.AddTransient<ILoadItemService, LoadItemService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllersWithViews();

            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = 500 * 1024 * 1024;
            });
            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = 500 * 1024 * 1024;
                options.MultipartBodyLengthLimit = 500 * 1024 * 1024; ;
                options.MultipartHeadersLengthLimit = 500 * 1024 * 1024;
            });
            services.AddSingleton<ConfigurationHelper>();

            services.AddMvc()
          .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TestZLContext context)
        {

            context.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseAuthorization();
            app.Use(async (context, next) =>
            {
                context.Features.Get<IHttpMaxRequestBodySizeFeature>().MaxRequestBodySize = null;
                await next.Invoke();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
