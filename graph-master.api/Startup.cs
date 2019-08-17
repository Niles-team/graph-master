using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using graph_master.models.helpers;
using graph_master.data;
using graph_master.data.interfaces;
using graph_master.models.enums;
using graph_master.services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using graph_master.api.Helpers;

namespace graph_master.api
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
            services.AddCors(options => {
                options.AddDefaultPolicy(builder => {
                    builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
                });
            });

            services.AddHttpContextAccessor();
            services.AddMvc(options =>
            {
                options.InputFormatters.Insert(0, new DataContractInputFormatter());
                options.OutputFormatters.Insert(0, new DataContractOutputFormatter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            string connectionString = Configuration.GetConnectionString("graph-master");
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            
            IDaoFactory factory = DaoFactories.GetFactory(DataProvider.AdoNet, connectionString);
            Environment.SetEnvironmentVariable("connectionString", connectionString);
            
            services.AddScoped<EmailService>(provider => {
                var appSettingsResolved = provider.GetService<IOptions<AppSettings>>();
                return new EmailService(appSettingsResolved.Value); 
            });

            services.AddTransient<UserService>(provider => {
                var appSettingsResolved = provider.GetService<IOptions<AppSettings>>();
                var emailService = provider.GetService<EmailService>();
                var httpAccessor = provider.GetService<IHttpContextAccessor>();
                return new UserService(factory.UserDao, emailService, appSettingsResolved.Value, httpAccessor);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
