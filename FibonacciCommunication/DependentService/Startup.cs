using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services.Configuration;
using Services.Implementations;
using Services.Interfaces;

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

            var rabbitConfig = Configuration.GetSection("RabbitConfiguration").Get<RabbitConfig>();
            //var httpConfig = Configuration.GetSection("HttpConfig").Get<HttpSenderConfig>();

            services.AddSingleton(rabbitConfig)
                //.AddSingleton(httpConfig)
                .AddSingleton<CalculationService>();

            services.AddSingleton<IMessageSender, RabbitSender>();
            services.AddSingleton<ICommunicationService, CommunicationService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
