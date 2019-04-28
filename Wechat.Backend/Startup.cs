using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Wechat.Backend.Areas.ServiceAccount.Filters;
using Wechat.Backend.Areas.ServiceAccount.ModelBinders;
using Wechat.Backend.Areas.ServiceAccount.Models;

namespace Wechat.Backend
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);
            builder.AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //允许跨域
            services.AddCors();

            services
                .AddMvc(options =>
                {
                    options.ModelBinderProviders.Insert(0, new WechatMessageBaseEntityBinderProvider());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var connectionString = Configuration.GetSection("ConnectionString").Value;
            services.AddDbContext<WechatDbContext>(options => options.UseSqlServer(connectionString));

            //添加Swagger.
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "微信后端平台", Version = "v1" }); });

            services.AddTransient<LogRequestFilter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "areas",
                    "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}"
                );
            });

            loggerFactory.AddLog4Net("log4net.config");

            //配置Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "DemoAPI V1"); });

            // 自动检测数据库并升级
            AutoMigration(app);
        }

        private void AutoMigration(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<WechatDbContext>();

                context.Database.Migrate();
            }
        }
    }
}