using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Services;
using System.Collections.Generic;

namespace DependentService
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
            services.AddControllers();

            var rabbitConfig = GetRabbitConnectionConfiguration(Configuration);
            services.AddSingleton(rabbitConfig);
            services.AddSingleton<CalculationService>();

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Dependent service api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

        private static ConnectionConfiguration GetRabbitConnectionConfiguration(IConfiguration configuration)
        {
            var rabbitConnectionConfigurationSection = configuration.GetSection("RabbitConnectionConfiguration");

            var connectionConfiguration = rabbitConnectionConfigurationSection.Get<ConnectionConfiguration>();
            connectionConfiguration.Hosts = rabbitConnectionConfigurationSection.GetSection("Hosts").Get<IEnumerable<HostConfiguration>>();

            return connectionConfiguration;
        }
    }
}
