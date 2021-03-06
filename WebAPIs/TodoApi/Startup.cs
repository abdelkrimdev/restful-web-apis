﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts;
using Shared.MongoDb;
using Swashbuckle.AspNetCore.Swagger;

namespace TodoApi
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
            services
                .Configure<MongoSettings>(
                    (options) =>
                    {
                        options.Host = Configuration.GetValue<string>("TODO_MONGO_HOST");
                        options.Port = Configuration.GetValue<string>("TODO_MONGO_PORT");
                        options.User = Configuration.GetValue<string>("TODO_MONGO_USER");
                        options.Pass = Configuration.GetValue<string>("TODO_MONGO_PASS");
                        options.Data = Configuration.GetValue<string>("TODO_MONGO_DB");
                    }
                )
                .AddTransient(
                    serviceType: typeof(IRepository<>),
                    implementationType: typeof(MongoRepository<>)
                )
                .AddSwaggerGen(options => options.SwaggerDoc("v1", new Info { Title = "Todo API", Version = "v1" }))
                .AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(
                (options) =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API");
                    options.RoutePrefix = string.Empty;
                }
            );

            app.UseMvc();
        }
    }
}
