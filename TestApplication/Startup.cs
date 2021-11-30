using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using TestApplication.Data.DbModels;
using TestApplication.Data;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;
using TestApplication.Data.Other;

namespace TestApplication
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
            services.AddCors();
            services.AddControllersWithViews();
            services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("InMemDb"));
            services.AddLogging();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<JwtService>();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                    
                }
            });

            app.UseCors(options => {
                options
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials() //Allows front end to receive cookies.
                .WithOrigins(new[] { "http://localhost:3000"});

                     });

            using (var serviceScope = app.ApplicationServices.CreateScope()) {
                var context = serviceScope.ServiceProvider.GetService<DataContext>();
                AddTestData(context);
            }
            
        }

        private void _AddTestData<T>(DataContext dbContext,string filename) {
            using (StreamReader file = File.OpenText(@"./Data/"+filename))
            {
                string s = file.ReadToEnd();
                
                T[] data = (T[])JsonSerializer.Deserialize(s, typeof(T[]));
                
                foreach (T datum in data)
                {
                    dbContext.Add(datum);

                }
                
            }
            dbContext.SaveChanges();
        }

        public void AddTestData(DataContext dbContext) {
            _AddTestData<Account>(dbContext, "Account.json");
            _AddTestData<MonitorData>(dbContext, "MonitorData.json");
            _AddTestData<QueueGroup>(dbContext, "QueueGroup.json");

        }
    }
}
