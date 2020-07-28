using API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using AutoMapper;
using API.Helpers;

namespace API
{
  public class Startup
  {
    private readonly IConfiguration _config;
    public Startup(IConfiguration configuration)
    {
      _config = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();

      //For manipulating the returned json to the client.
      services.AddAutoMapper(typeof(MappingProfiles));

      //Add the conn for sqlite db.
      services.AddDbContext<StoreContext>(x => x.UseSqlite(_config.GetConnectionString("DefaultConnection")));

      //Just tool to test api, json to ts.
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo {Title="Demo API", Version="v1"});
      });

      //Serving cors.
      services.AddCors(opt => 
      {
        opt.AddPolicy("CorsPolicy", policy =>
        {
          policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
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

      app.UseRouting();
      app.UseStaticFiles();

      app.UseCors("CorsPolicy");
      app.UseAuthorization();
      app.UseSwagger();
      app.UseSwaggerUI(c => 
      { 
        c.SwaggerEndpoint("/swagger/v1/swagger.json","Demo API v1");
      });

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
