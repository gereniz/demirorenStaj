using System;
using System.Text;
using System.Threading.Tasks;
using DemirorenBlog.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace DemirorenBlog.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<DomainContext>(options => {
                options.UseSqlServer(Configuration.GetSection("ConnectionString").Value);
            });

            //services.AddSingleton<IDIService, DIService>();
            services.AddMemoryCache();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                     options =>
                     {
                        options.LoginPath = new PathString("/Home/login");
                      
        });
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
            )
            
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidAudience = "",
                    ValidateIssuer = false,
                    ValidIssuer = "",
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("GüştaGerenizSüleymanDemirelÜniversitesiDemirorenTeknoloji"))
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = ctx => {
                   
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = ctx => {
                        Console.WriteLine("Exception:{0}", ctx.Exception.Message);
                        return Task.CompletedTask;
                    }
                };
            });
        }

  
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }
        app.UseAuthentication();
        app.UseHttpsRedirection();
        app.UseMvc(routes =>
        {
            routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}");
        });

        }
        
        
        
    }
}




