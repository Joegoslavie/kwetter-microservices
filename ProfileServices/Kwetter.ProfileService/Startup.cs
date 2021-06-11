﻿// <auto-generated />
using Confluent.Kafka;
using Kwetter.Messaging;
using Kwetter.ProfileService.EventHandlers;
using Kwetter.ProfileService.Persistence.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kwetter.ProfileService
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddDbContext<ProfileContext>(o => o.UseSqlServer(this.configuration.GetConnectionString("kwetter-profile-db")));

            ConsumerConfig config = new ConsumerConfig
            {
                BootstrapServers = configuration.GetValue<string>("ProducerConfiguration:Servers"),
                GroupId = configuration.GetValue<string>("ProducerConfiguration:GroupId"),
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
            };

            try
            {
                var context = services.BuildServiceProvider().GetRequiredService<ProfileContext>();
                context.Database.Migrate();
            }
            catch
            {

            }

            var builder = new ConsumerBuilder<Ignore, string>(config).Build();

            builder.Subscribe(EventSettings.NewProfileEventTopic);
            services.AddHostedService(sp => new KafkaEventHandler(builder, services.BuildServiceProvider().GetRequiredService<ProfileContext>()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ProfileContext>();
                context.Database.EnsureCreated();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<Services.ProfileService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
