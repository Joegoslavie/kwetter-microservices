﻿// <auto-generated />
namespace Kwetter.AuthenticationService
{
    using Confluent.Kafka;
    using Kwetter.AuthenticationService.Persistence.Context;
    using Kwetter.AuthenticationService.Persistence.Entity;
    using Kwetter.Messaging;
    using Kwetter.Messaging.Events;
    using Kwetter.Messaging.Interfaces;
    using Kwetter.Messaging.Interfaces.Tweet;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using System.Collections.Generic;
    using System.Text;

    public class Startup
    {
        private ILogger<Startup> logger;
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
            services.AddDbContext<AuthenticationContext>(o => o.UseSqlServer(this.configuration.GetConnectionString("kwetter-auth-db")));
            services.AddIdentity<KwetterUserEntity<int>, KwetterRoleEntity<int>>()
                .AddEntityFrameworkStores<AuthenticationContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Kwetter has a VERY strong password policy.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 2;
                options.Password.RequiredUniqueChars = 1;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = this.configuration["JWT:ValidAudience"],
                    ValidIssuer = this.configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["JWT:Secret"])),
                };
            });

            var builder = new ProducerBuilder<string, string>(new ProducerConfig {
                BootstrapServers = "kafka-service:9092",
            }).Build();

            services.AddSingleton<IProfileEvent>(_ => new ProfileEvent(builder, new List<string>
            {
                EventSettings.ProfileEventTopic,
                EventSettings.TweetProfileRefEventTopic,
                EventSettings.FollowRefProfileEventTopic,
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline. 
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            this.logger = app.ApplicationServices.GetService<ILogger<Startup>>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AuthenticationContext>();
                this.logger.LogInformation($"Running EF EnsureCreated method..");
                var result = context.Database.EnsureCreated();

                this.logger.LogInformation($"...done, result: {result}");
            }

            this.logger.LogInformation($"Kafka address: {this.configuration.GetValue<string>("ProducerConfiguration:Servers")}");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<Services.AuthenticationService>();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
