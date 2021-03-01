using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using MyMusic.Core;
using MyMusic.Data;
using MyMusic.Data.MongoDB;
using MyMusic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.API
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

            services.AddMvc(o => o.EnableEndpointRouting = false).AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            // SQL Server Config
            services
                .AddDbContext<MyMusicDbContext>(
                                                options => options.UseSqlServer(
                                                                        Configuration.GetConnectionString("Default"),
                                                                        x => x.MigrationsAssembly("MyMusic.Data")
                                                                        )
                                                );

            // MongoDB Config
            services.Configure<Setting>(
                options =>
                {
                    options.ConnectionString = Configuration.GetValue<String>("MongoDB:ConnectionString");
                    options.Database = Configuration.GetValue<String>("MongoDB:Database");
                });

            // **************************** Injection de d�pendances************************
            // Une seule instance MongoDb pour toute l'application
            services.AddSingleton<IMongoClient, MongoClient>(
                _ => new MongoClient(Configuration.GetValue<String>("MongoDB:ConnectionString"))
                );

            // Cr�er une instance UnitOfWork pour chaque requette HTTP
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // Cr�er une instance ComposerRepository pour chaque requette HTTP
            services.AddScoped<IComposerRepository, ComposerRepository>();

            services.AddTransient<IDatabaseSettings, DatabaseSettings>();

            // Cr�er une instance pour chaque Appel au controller
            services.AddTransient<IMusicService, MusicService>();
            services.AddTransient<IArtistService, ArtistService>();
            services.AddTransient<IComposerService, ComposerService>();
            services.AddTransient<IUserService, UserService>();

            // Swagger
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "First Swagger Music API",
                    Description = "Music API Swagger - Version 1"
                });
            });

            // AutoMapper
            services.AddAutoMapper(typeof(Startup));

            // JWT Authentication
            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("AppSettings:Secret"));
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(auth =>
            {
                auth.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        var userId = int.Parse(context.Principal.Identity.Name);
                        var user = userService.GetUserById(userId);

                        if (user == null)
                        {
                            context.Fail("Unauthorized");
                        }

                        return Task.CompletedTask;
                    }
                };

                auth.RequireHttpsMetadata = false;
                auth.SaveToken = true;
                auth.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline (MiddleWares).
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Swagger Middleware
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Music API V1");
            });

        }
    }
}
