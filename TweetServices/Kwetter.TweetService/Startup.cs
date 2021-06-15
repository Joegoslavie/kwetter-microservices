﻿// <auto-generated />
using Confluent.Kafka;
using Kwetter.Messaging;
using Kwetter.Messaging.Events;
using Kwetter.Messaging.Interfaces.Tweet;
using Kwetter.TweetService.Business;
using Kwetter.TweetService.EventHandler;
using Kwetter.TweetService.Persistence.Context;
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

namespace Kwetter.TweetService
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
            services.AddDbContext<TweetContext>(o => o.UseSqlServer(this.configuration.GetConnectionString("kwetter-tweet-db")));
            services.AddTransient<TweetManager>();

            ConsumerConfig config = new ConsumerConfig
            {
                BootstrapServers = configuration.GetValue<string>("ProducerConfiguration:Servers"),
                GroupId = configuration.GetValue<string>("ProducerConfiguration:GroupId"),
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
            };

            Console.WriteLine($"Using kafka endpoint: {config.BootstrapServers}");

            var builder = new ConsumerBuilder<Ignore, string>(config).Build();
            builder.Subscribe(new List<string> 
            {
                EventSettings.NewTweetProfileEventTopic,
                EventSettings.TweetProfileUpdateEventTopic,
                EventSettings.NewTweetMentionEventTopic,
            });

            var producer = new ProducerBuilder<string, string>(config).Build();
            services.AddSingleton<ITweetEvent>(_ => new TweetMentionEvent(producer, EventSettings.NewTweetMentionEventTopic));
            services.AddHostedService(sp => new KafkTweetEventHandler(builder, services.BuildServiceProvider().GetRequiredService<TweetContext>()));
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
                var context = serviceScope.ServiceProvider.GetService<TweetContext>();
                context.Database.EnsureCreated();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<Services.TweetService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
