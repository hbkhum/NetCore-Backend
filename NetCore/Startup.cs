using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NetCore.DependencyInjection;
using NetCore.Hub;
using NetCore.Infrastructure.Context;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NetCore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc();
            services.AddSignalR().AddJsonProtocol(options =>
            {
                options.PayloadSerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        //.WithOrigins("http://localhost:4200")
                        .AllowAnyOrigin()
                        );
            });


            services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.Stores.MaxLengthForKeys = 128)
                .AddEntityFrameworkStores<VehicleContext>()
                .AddDefaultTokenProviders();

            //services.AddDbContext<VehicleContext>(options => options.UseMySql(Configuration.GetConnectionString("VehicleContext")));
            services.AddDbContext<VehicleContext>(options => options.UseMySql("Server=192.168.2.200; Port=3306; Database=VehicleContext; Uid=root; Pwd=black; SslMode=Preferred;"));

            //services.AddDbContext<NavaContext>(options => options.UseMySql("server=ncs-database.mysql.database.azure.com;user id=ncs;password=Talikajoer1;database=NAVAContext;persistsecurityinfo=True;Port=3306;"));
            //services.AddDbContext<NavaContext>(options => options.UseMySql("Server=ncs-database.mysql.database.azure.com; Port=3306; Database=NAVAContext; Uid=ncs@ncs-database; Pwd=Talikajoer1; SslMode=Preferred;"));

            //"server=192.168.1.214;user id=root;password=black;database=NAVAContext;persistsecurityinfo=True;Port=3306;"
            services.RegisterRepositories();
            services.RegisterServices();
            services.RegisterDataRepositories();
            services.RegisterDataServices();

            services.AddMvc().AddJsonOptions(jsonOptions =>
            {
                jsonOptions.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                jsonOptions.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                jsonOptions.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            services.AddAuthentication((option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }))
                .AddJwtBearer(options =>
                {

                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "yourdomain.com",
                        ValidAudience = "yourdomain.com",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecureKey")),
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSignalR(routes =>
            {
                routes.MapHub<CarHub>("/hubs/car");
                routes.MapHub<TruckHub>("/hubs/truck");
            });

            //app.UseSpaStaticFiles();

            app.UseMvc();
        }
    }
}
