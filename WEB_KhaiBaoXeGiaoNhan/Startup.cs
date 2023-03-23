using BearerHelper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Models.Common;
using System;
using System.Threading.Tasks;

namespace WEB_KhaiBaoXeGiaoNhan
{
    public class Startup
    {
        private Config config = Config.getInstance();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            config.connWeb = Configuration.GetConnectionString("Web_BookingTrans");
            config.connPMC = Configuration.GetConnectionString("VAS_4000");
            config.connPMC3000 = Configuration.GetConnectionString("VAS_3000");
            config.connPMC6000 = Configuration.GetConnectionString("VAS_6000");
            config.appSecret = Configuration["AppSettings:Secret"];
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Cors

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });

            #endregion Cors

            services.AddControllersWithViews();

            #region Authentication

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            #region kiểm tra issuer và audience

                            ValidateIssuer = false,
                            ValidateAudience = false,

                            #endregion kiểm tra issuer và audience

                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = "Fiver.Security.Bearer",
                            ValidAudience = "Fiver.Security.Bearer",
                            IssuerSigningKey = JwtSecurityKey.Create(config.appSecret)
                        };

                        options.Events = new JwtBearerEvents
                        {
                            OnAuthenticationFailed = context =>
                            {
                                Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                                return Task.CompletedTask;
                            },
                            OnTokenValidated = context =>
                            {
                                Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                                return Task.CompletedTask;
                            }
                        };
                    });

            //optional, nếu phân chia nhiều policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin",
                    policy => policy.RequireClaim("SuperID"));
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("normal",
                    policy => policy.RequireClaim("NormalID"));
            });

            services.AddMvc();

            #endregion Authentication

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "VehicleRegistẻ API",
                    Description = "A ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "HoangNM",
                        Email = string.Empty,
                        Url = new Uri("https://google.com"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://google.com"),
                    }
                });
            });
            services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // global cors policy

            #region Cors

            app.UseCors(option => option.AllowAnyOrigin());

            #endregion Cors

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            #region Authentication

            app.UseAuthentication();
            app.UseAuthorization();

            #endregion Authentication

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}