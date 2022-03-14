using Checkout.PaymentGateway.DomainServices;
using Checkout.PaymentGateway.DomainServices.Core;
using Checkout.PaymentGateway.Infrastructure.Core.ApiClients;
using Checkout.PaymentGateway.Infrastructure.Core.Encryptors;
using Checkout.PaymentGateway.Infrastructure.Core.Repositories;
using Checkout.PaymentGateway.Infrastructure.Cryptography.Configuration;
using Checkout.PaymentGateway.Infrastructure.Cryptography.Encryptors;
using Checkout.PaymentGateway.Infrastructure.Http.Clients;
using Checkout.PaymentGateway.Infrastructure.Services;
using Checkout.PaymentGateway.Infrastructure.Services.Core;
using Checkout.PaymentGateway.Infrastructure.Sql.Context;
using Checkout.PaymentGateway.Infrastructure.Sql.Repositories;
using Checkout.PaymentGateway.WebApi.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Prometheus;
using System.Collections.Generic;
using System.Configuration;
using System.Text.Json;

namespace Checkout.PaymentGateway.WebApi
{
	public class Startup
	{
		public IConfiguration Configuration { get; }
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;

		}
		public void ConfigureServices(IServiceCollection services)
		{
            AddSwagger(services);

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                }); 

            services.AddDbContext<PaymentGatewayContext>(options =>
			   options.UseSqlServer(Configuration.GetConnectionString("PaymentGatewayConnectionString")));
			
			services.AddScoped<IPaymentService, PaymentService>();
			services.AddScoped<IPaymentInfrastructureService, PaymentInfrastructureService>();
			services.AddScoped<IPaymentRepository, PaymentRepository>();
			services.AddScoped<IBankClient, MockBankClient>();
            services.AddScoped<ICryptographyService, CryptographyService>();
            services.AddSingleton<ICryptographyConfiguration, CryptographyConfiguration>();
        }

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			app.UseHttpsRedirection();
			app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment Gateway API");
            });

            app.UseCustomExceptionHandler();
            app.UseMetricServer();
            app.UseHttpMetrics();

            app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
              
			});
        ;
        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Payment Gatewayd API",

                });
            });
        }
    }
}
