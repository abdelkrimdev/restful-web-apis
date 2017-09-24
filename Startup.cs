using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SupaTrupa.WebAPI.Settings;
using SupaTrupa.WebAPI.Shared.Contracts;
using SupaTrupa.WebAPI.Shared.MongoDb;

namespace SupaTrupa.WebAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
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
                .AddCors()
                .AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory
                .AddConsole(Configuration.GetSection("Logging"))
                .AddDebug();

            app
                .UseCors(
                    (options) =>
                        options
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()
                )
                .UseMvc();
        }
    }
}
