using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.Configuration;
using Services.Implementations;
using Services.Interfaces;

namespace InitiatorService
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
            var httpConfig = Configuration.GetSection("HttpConfig").Get<HttpSenderConfig>();

            services.AddSingleton(rabbitConfig)
                .AddSingleton(httpConfig)
                .AddSingleton<CalculationService>();

            services.AddHttpClient<IMessageSender, HttpSender>();
            services.AddSingleton<ICommunicationService, CommunicationService>();

            services.AddHostedService<WorkerService>();
            //services.AddSingleton<WorkerService>();
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
