using System;
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
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ESLog;
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

            //var connSqlSerer = Configuration.GetConnectionString("BlogDb_SQLServer");
            //services.AddDbContext<BloggingContext>(opt => opt.UseSqlServer(connSqlSerer, b => b.MigrationsAssembly("Data")));

            //var connSqlMySql = Configuration.GetConnectionString("BlogDb_MySQL");
            var connSqlMySql = "server=192.168.18.20;user id=cnbooking;password=cnbooking;persistsecurityinfo=True;database=Bloger;sslmode=None";
            //Mysql官方驱动，有问题
            //services.AddDbContext<BloggingContext>(opt => opt.UseMySQL(connSqlMySql, b => b.MigrationsAssembly("Data")));
            //Mysql第三方驱动 Pomelo, 未设置自增长(因为最先是通过 SQLServer 生成的，删除 Migrations 后，切换到 MYSQL 下，重新迁移，就可以了)
            services.AddDbContext<BloggingContext>(opt => opt.UseMySql(connSqlMySql, b => b.MigrationsAssembly("Data")));

            services.AddAuthorization(a => {
                var b = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser()
                    .Build();
                a.AddPolicy("Bearer", b);
            });

            services.AddMvc();

            services.AddTransient<IBlogs, BlogsImpl>();
        }




        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, BloggingContext ctx) {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddProvider(new ESLogProvider());

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();


            #region exception
            app.UseExceptionHandler(o => {
                o.Use(async (context, next) => {
                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;
                    if (error != null) {
                        if (error.Error is SecurityTokenExpiredException) {
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";

                            await context.Response.WriteAsync(JsonConvert.SerializeObject(
                                new { authenticated = false, tokenExpired = true }
                            ));
                        } else {
                            context.Response.StatusCode = 500;
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(
                                new { success = false, error = error.Error.Message }
                            ));
                        }
                    } else {
                        await next();
                    }
                });
            });
            #endregion

            #region Jwt
            app.UseJwtBearerAuthentication(new JwtBearerOptions {
                TokenValidationParameters = new TokenValidationParameters {
                    IssuerSigningKey = TokenAuthOption.Key,
                    ValidAudience = TokenAuthOption.Audience,
                    ValidIssuer = TokenAuthOption.Issuer,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(0),

                }
            });
            #endregion

            #region 路由，必须在 Jwt 后面
            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            #endregion

            BlogDBInitilizer.Init(ctx);
        }
    }
}
