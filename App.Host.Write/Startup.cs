using System;
using System.Transactions;
using App.Host.Read.Consumer;
using App.Host.Read.Context;
using App.Host.Write.Context;
using MassTransit;
using MassTransit.AspNetCoreIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace App.Host.Write
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<ApplicationWriteDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<ApplicationReadDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });


                cfg.ReceiveEndpoint(e =>
                {
                    e.UseTransaction(x =>
                    {
                        x.Timeout = TimeSpan.FromSeconds(90);
                        x.IsolationLevel = IsolationLevel.ReadCommitted;
                    });

                    using var context = services.BuildServiceProvider().GetRequiredService<ApplicationReadDbContext>();
                   
                    e.Consumer(() => new WeatherForecastConsumer(context));
                });
            });

 

            services.AddSingleton(busControl);
            services.AddSingleton<IBus>(busControl);
            services.AddSingleton<IPublishEndpoint>(busControl);
            services.AddSingleton<ISendEndpointProvider>(busControl);

            busControl.StartAsync();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseRouting();


            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}