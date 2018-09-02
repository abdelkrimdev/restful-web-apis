using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts;
using Shared.MongoDb;

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
                        options.User = Configuration.GetValue<string>("Mongo:User");
                        options.Pass = Configuration.GetValue<string>("Mongo:Pass");
                        options.Host = Configuration.GetValue<string>("Mongo:Host");
                        options.Port = Configuration.GetValue<string>("Mongo:Port");
                        options.Data = Configuration.GetValue<string>("Mongo:Data");
                    }
                )
                .AddTransient(
                    serviceType: typeof(IRepository<>),
                    implementationType: typeof(MongoRepository<>)
                )
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
            app.UseMvc();
        }
    }
}
