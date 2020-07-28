using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.Extensions.Logging;

namespace API.Data
{
    public class StoreContextInitData
    {
        //Load the initial data from json files to the created db. 
        public static async Task PopulateDataAsync(StoreContext context, ILoggerFactory loggerFactory)
        {
            try 
            {
                //DiscountTypes
                if(!context.DiscountTypes.Any())
                {
                    var discountTypesData = File.ReadAllText("Data/InitData/discounttypes.json");
                    var discountTypes = JsonSerializer.Deserialize<List<DiscountType>>(discountTypesData);

                    foreach(var item in discountTypes)
                    {
                        context.DiscountTypes.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                //Discount
                if(!context.Discounts.Any())
                {
                    var discountData = File.ReadAllText("Data/InitData/discounts.json");
                    var discounts = JsonSerializer.Deserialize<List<Discount>>(discountData);

                    foreach(var item in discounts)
                    {
                        context.Discounts.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                //Packages / Events
                if(!context.Packages.Any())
                {
                    var packagesData = File.ReadAllText("Data/InitData/packages.json");
                    var packages = JsonSerializer.Deserialize<List<Package>>(packagesData);

                    foreach(var item in packages)
                    {
                        context.Packages.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                //Promos
                if(!context.Promos.Any())
                {
                    var promosData = File.ReadAllText("Data/InitData/promos.json");
                    var promos = JsonSerializer.Deserialize<List<Promo>>(promosData);

                    foreach(var item in promos)
                    {
                        context.Promos.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextInitData>();
                logger.LogError(ex, "Error occured during loading of init data from files to the db.");
            }
        }
    }
}