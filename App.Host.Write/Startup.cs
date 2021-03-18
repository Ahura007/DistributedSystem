using System;
using System.Transactions;
using App.Host.Read.Consumer;
using App.Host.Read.Context;
using App.Host.Write.Context;
using GreenPipes;
using GreenPipes.Introspection;
using MassTransit;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<ApplicationWriteDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<ApplicationReadDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            var db = services.BuildServiceProvider().GetRequiredService<ApplicationReadDbContext>();
 

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

                    e.Consumer(() => new WeatherForecastConsumer(db), c =>
                    {
                   
                    });
                });
            });
            services.AddSingleton(busControl);
            services.AddSingleton<IBus>(busControl);
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