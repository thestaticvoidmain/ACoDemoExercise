using System;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
  public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            //Create the database. This is based on the migrations file, if db does not exists.
            using (var scope = host.Services.CreateScope()) 
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try 
                {
                    var context = services.GetRequiredService<StoreContext>();
                    await context.Database.MigrateAsync();

                    //Begin loading initial data from file. 
                    await StoreContextInitData.PopulateDataAsync(context, loggerFactory);
                }
                catch(Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "Error occured during initialing db creation at startup.");
                }
            }

            //Proceed to run API application.
            host.Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
