using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TheGuardian.Core.Interfaces;
using TheGuardian.DataAccess;

[assembly : ApiController]
namespace TheGuardianAPI {
    public class Startup {
        private const string CorsPolicyName = "AllowConfiguredOrigins";

        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices (IServiceCollection services) {
            // switch between database providers using runtime configuration
            // (the existing migrations are SQL-Server-specific, but the model itself is not)

            // this should be the name of a connection string.
            string whichDb = Configuration["DatabaseConnection"];
            if (whichDb is null) {
                throw new InvalidOperationException ($"No value found for \"DatabaseConnection\"; unable to connect to a database.");
            }

            string connection = Configuration.GetConnectionString (whichDb);
            if (connection is null) {
                throw new InvalidOperationException ($"No value found for \"{whichDb}\" connection; unable to connect to a database.");
            }

            if (whichDb.Contains ("PostgreSql", StringComparison.InvariantCultureIgnoreCase)) {
                services.AddDbContext<GuardianContext> (options =>
                    options.UseNpgsql (connection));
            } else {
                services.AddDbContext<GuardianContext> (options =>
                    options.UseSqlServer (connection));
            }

            services.AddScoped<IGuardianRepository, GuardianRepository> ();

            services.AddSwaggerGen (c => {
                c.SwaggerDoc ("v1", new OpenApiInfo { Title = "Guardian API", Version = "v1" });
            });

            // support switching between database providers using runtime configuration

            var allowedOrigins = Configuration.GetSection ("CorsOrigins").Get<string[]> ();

            services.AddCors (options => {
                options.AddPolicy (CorsPolicyName, builder =>
                    builder.WithOrigins (allowedOrigins ?? Array.Empty<string> ())
                    .AllowAnyMethod ()
                    .AllowAnyHeader ()
                    .AllowCredentials ());
            });

            services.AddControllers (options => {
                // remove the default text/plain string formatter to clean up the OpenAPI document
                options.OutputFormatters.RemoveType<StringOutputFormatter> ();

                options.ReturnHttpNotAcceptable = true;
                options.SuppressAsyncSuffixInActionNames = false;
            });
        }

        public void Configure (IApplicationBuilder app, IWebHostEnvironment env, GuardianContext guardianContext) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            if (Configuration.GetValue ("UseHttpsRedirection", defaultValue : true) is true) {
                app.UseHttpsRedirection ();
            }

            app.UseRouting ();

            app.UseCors (CorsPolicyName);

            app.UseAuthorization ();

            app.UseSwagger ();

            app.UseSwaggerUI (c => {
                c.SwaggerEndpoint ("/swagger/v1/swagger.json", "Guardian API V1");
            });

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });
        }
    }
}