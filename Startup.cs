using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SupaTrupa.WebAPI.AppSettings;
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
                .Configure<MongoDbSettings>(
                    (options) =>
                    {
                        options.User = Configuration.GetValue<string>("MongoDb:User");
                        options.Pass = Configuration.GetValue<string>("MongoDb:Pass");
                        options.Host = Configuration.GetValue<string>("MongoDb:Host");
                        options.Port = Configuration.GetValue<string>("MongoDb:Port");
                        options.Data = Configuration.GetValue<string>("MongoDb:Data");
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
