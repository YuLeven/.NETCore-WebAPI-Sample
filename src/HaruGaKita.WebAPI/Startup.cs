using System;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using HaruGaKita.Persistence;
using HaruGaKita.Persistence.Interfaces;
using HaruGaKita.Infrastructure.Data;
using MediatR;
using HaruGaKita.WebAPI.Error;
using System.Threading.Tasks;
using HaruGaKita.Application.Accounts.Commands;

#pragma warning disable 1591
namespace HaruGaKita.WebAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.local.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("HaruGaKitaDB");
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateAccountCommandValidator>());


            services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<HaruGaKitaDbContext>(options => options.UseNpgsql(connectionString))
                .AddScoped(typeof(IAsyncRepository<>), typeof(EntityFrameworkRepository<>))
                .AddMediatR(typeof(HaruGaKita.Application.Accounts.Commands.CreateAccountCommand).Assembly);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
            {
                options.Authority = Common.Configuration.AppAuthority;
                options.Audience = Common.Configuration.ApiAudience;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = Common.Configuration.ApplicationSecurityKey,
                    ValidIssuer = Common.Configuration.AppAuthority,
                    ValidateAudience = true,
                    ValidAudience = Common.Configuration.ApiAudience
                };
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Haru ga Kita! - Core",
                    Version = "v1"
                });

                var xmlFile = Path.ChangeExtension(typeof(Startup).Assembly.Location, ".xml");
                options.IncludeXmlComments(xmlFile);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Haru ga Kita!");
                });
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    await Task.Run(() =>
                    {
                        var apiErrorHandler = new ApiErrorHandler(context);
                        apiErrorHandler.Handle();
                    });
                });
            });
            app.UseMvc();
        }
    }

    public static class HttpExtensions
    {
        private static readonly JsonSerializer Serializer = new JsonSerializer
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        public static void WriteJson<T>(this HttpResponse response, T obj, string contentType = null)
        {
            response.ContentType = contentType ?? "application/json";
            using (var writer = new HttpResponseStreamWriter(response.Body, Encoding.UTF8))
            {
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    jsonWriter.CloseOutput = false;
                    jsonWriter.AutoCompleteOnClose = false;

                    Serializer.Serialize(jsonWriter, obj);
                }
            }
        }
    }
}
