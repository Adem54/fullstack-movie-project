using AutoMapper;
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
using Microsoft.OpenApi.Models;
using MoviesAPI.ApiBehavior;
using MoviesAPI.Filters;
using MoviesAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MoviesAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration=configuration;
        }
        public IConfiguration Configuration { get; }
  
        public void ConfigureServices(IServiceCollection services)
        {
            //Biz bu database imizi, yani DbContext ten turetilecek olan ApplicationDbContext i
            //tum uygulama tum Api controller daki, endpontler tarafindan kullanacagimz icin
            //bu sekilde options ile bir konfigurasyon yapariz..
            services.AddDbContext<ApplicationDbContext>(options=>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );
            
            services.AddAutoMapper(typeof(Startup));    

            services.AddScoped<IFileStorageService,AzureStorageService>();

            services.AddControllers(options=>
            {
                 options.Filters.Add(typeof(MyExceptionFilter));   
                 options.Filters.Add(typeof(ParseBadRequest));
            }).ConfigureApiBehaviorOptions(BadRequestBehavior.Parse);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddSwaggerGen();
            services.AddResponseCaching();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            services.AddCors(options=>{
                var frontendURL=Configuration.GetValue<string>("frontend_url");
                // var frontEndUrl=Configuration["frontend_url"];
                options.AddDefaultPolicy(builder=>{
                    builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader()
                    .WithExposedHeaders(new string[] {"totalAmountOfRecords"});
                });
            });   
            //Cors ta bir konfigurasyon yapmamiz gerekiyor, cunku HttpContexExtensions da yazdigmiz
            //     httpContext.Response.Headers.Add("totalAmountOfRecords",count.ToString()); burda ki totalAmountOfRecords bu stringin
            //Cors un bu bilginin WEbbrowser da client ta okunmasina izin vermek icin bir konfigurasyon yapariz..
            //   .WithExposedHeaders(new string[] {"totalAmountOfRecords"}); burayi ekleriz...
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints((endpoints) =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

