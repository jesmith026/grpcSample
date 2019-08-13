using System;
using System.Linq;
using grpcSample.Server.Data;
using grpcSample.Server.Data.Entities;
using grpcSample.Server.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Socratic.DataAccess.DependencyInjection;

namespace grpcSample.server
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
            services.AddDbContextPool<SchoolContext>(opt => {
                opt.UseSqlServer(@"Data Source=db;Initial Catalog=School;User Id=SA;Password=P@ssw0rd");
            });

            services.AddGrpc();
            services.AddSocraticDataAccess<SchoolContext>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            app.UpdateDb();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<StudentService>();
            });
        }
    }

    internal static class Extensions {

        public static void UpdateDb(this IApplicationBuilder app) {

            using var scope = app.ApplicationServices.CreateScope();
            var provider = scope.ServiceProvider;
            var db = provider.GetService<SchoolContext>();
            if (db.Database.GetPendingMigrations().Any())
                db.Database.Migrate();

            if (!db.Students.Any()) {
                for (var i = 0; i < 20; i++) {
                    var student = new Student {Name = Guid.NewGuid().ToString()};
                    db.Students.Add(student);
                }

                db.SaveChanges();
            }

        }

    }
}
