﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Entity;
using IBiz;
using Biz;
using Data;
//using MySQL.Data.EntityFrameworkCore.Extensions;

namespace Web {
    public class Startup {

        public IConfigurationRoot Configuration { get; }


        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment()) {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            Configuration = builder.Build();
        }









        public void ConfigureServices(IServiceCollection services) {
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc();

            //var connSqlSerer = Configuration.GetConnectionString("BlogDb_SQLServer");
            //services.AddDbContext<BloggingContext>(opt => opt.UseSqlServer(connSqlSerer, b => b.MigrationsAssembly("Data")));

            //var connSqlMySql = Configuration.GetConnectionString("BlogDb_MySQL");
            var connSqlMySql = "server=192.168.18.20;user id=cnbooking;password=cnbooking;persistsecurityinfo=True;database=Bloger;sslmode=None";
            //Mysql官方驱动，有问题
            //services.AddDbContext<BloggingContext>(opt => opt.UseMySQL(connSqlMySql, b => b.MigrationsAssembly("Data")));
            //Mysql第三方驱动 Pomelo, 未设置自增长(因为最先是通过 SQLServer 生成的，删除 Migrations 后，切换到 MYSQL 下，重新迁移，就可以了)
            services.AddDbContext<BloggingContext>(opt => opt.UseMySql(connSqlMySql, b => b.MigrationsAssembly("Data")));

            services.AddTransient<IBlogs, BlogsImpl>();
        }




        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, BloggingContext ctx) {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            BlogDBInitilizer.Init(ctx);
        }
    }
}
