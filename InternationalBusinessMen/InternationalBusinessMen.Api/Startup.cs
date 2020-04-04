using System;
using AutoMapper;
using InternationalBusinessMen.Api.Services;
using InternationalBusinessMen.Core.IRepositories;
using InternationalBusinessMen.Core.IServices;
using InternationalBusinessMen.Core.Services;
using InternationalBusinessMen.Infraestructure;
using InternationalBusinessMen.Infraestructure.ExternalProviders;
using InternationalBusinessMen.Infraestructure.ExternalServices;
using InternationalBusinessMen.Infraestructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;

namespace InternationalBusinessMen.Api
{
    public class Startup
    {
        private string _policyEverybodyWelcome = "EverybodyWelcome";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IExchangeService, ExchangeService>();
            services.AddScoped<IProductTransactionService, ProductTransactionService>();
            services.AddScoped<IRateService, RateService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IRateRepository, RateRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
   

            services.AddHttpClient<IRateExternalProvider, RateExternalProvider>(c =>
            {
                c.BaseAddress = new Uri(Configuration["Endpoints:Rates"]);
                c.Timeout = new TimeSpan(0, 0, 45);
                c.DefaultRequestHeaders.Clear();
            });

            services.AddHttpClient<ITransactionExternalProvider, TransactionExternalProvider>(c =>
            {
                c.BaseAddress = new Uri(Configuration["Endpoints:Transactions"]);
                c.Timeout = new TimeSpan(0, 0, 45);
                c.DefaultRequestHeaders.Clear();
            });

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(
                    Configuration.GetConnectionString("DbConnection")));

            services.AddCors(options =>
            {
                options.AddPolicy(_policyEverybodyWelcome,
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            services.AddControllers();

            services.AddAutoMapper(typeof(Startup));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                { 
                    Title = "International Business Men API", 
                    Version = "v1" 
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "International Business Men API");
                c.RoutePrefix = string.Empty;
            });


            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(_policyEverybodyWelcome);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
