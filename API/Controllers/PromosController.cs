using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Data;
using System;

namespace API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class PromosController : ControllerBase
  {
    private readonly StoreContext _context;

    public PromosController(StoreContext context)
    {
      _context = context;
    }

    [HttpPost("applyPromo")]
    public async Task<ActionResult<List<DiscountApplied>>> ApplyPromoAsync([FromBody] List<BasketItem> orders)
    {
      List<ActivePromo> promos = new List<ActivePromo>();
      await using (var command = _context.Database.GetDbConnection().CreateCommand())
      {
        command.CommandText = "SELECT p.PackageId, d.MinQuantity, d.DiscountValue, d.DiscountTypeId FROM Promos p INNER JOIN Discounts d ON p.DiscountId=d.Id INNER JOIN DiscountTypes dt ON d.DiscountTypeId=dt.Id";
        _context.Database.OpenConnection();
        using (IDataReader dr = command.ExecuteReader())
        {
          while (dr.Read())
          {
            promos.Add(new ActivePromo()
            {
              PackageId = Convert.ToInt32(dr[0]),
              MinQty = Convert.ToInt32(dr[1]),
              DiscountValue = Convert.ToInt32(dr[2]),
              DiscountTypeId = Convert.ToInt32(dr[3])
            });
          }
        }
      }

      List<DiscountApplied> discountOnBasketItems = new List<DiscountApplied>();
      
      //Loop for each item in the basket.
      foreach (BasketItem odr in orders)
      {
        List<ActivePromo> promosOnItem = promos.Where(p => p.PackageId == odr.PackageId && p.MinQty <= odr.Quantity).ToList<ActivePromo>();

        //Compute discount foreach promos on item. 
        List<DiscountApplied> discountsOnItem = new List<DiscountApplied>(); 
        foreach(ActivePromo promo in promosOnItem)
        {
            discountsOnItem.Add(ComputeDiscount(odr, promo));
        }

        if(promosOnItem.Count > 0)
        {
            //Best discount priority: 1. If quantity is higher, 2. discounted price is higher
            DiscountApplied bestDiscountOnItem = discountsOnItem.OrderByDescending(x => x.AddedQuantity).ThenByDescending(x => x.DiscountedPrice).First();
            discountOnBasketItems.Add(bestDiscountOnItem);
        }
      }

      return discountOnBasketItems;
    }

    private DiscountApplied ComputeDiscount(BasketItem item, ActivePromo promo)
    {
        DiscountApplied da = new DiscountApplied(item.PackageId);
        int discountMultiplier = item.Quantity / promo.MinQty;

        switch(promo.DiscountTypeId)
        { 
            case 1: //Dollar price reduction on item.
                da.DiscountedPrice = discountMultiplier * promo.DiscountValue;
                break;
            
            case 2: //Price reduction based on discount value in percent
                double discountPercent = (double)promo.DiscountValue / 100;
                da.DiscountedPrice = discountMultiplier * Convert.ToInt32(discountPercent * item.Price);
                
                break;
            case 3: //Increase the quantity based on discount value.
                da.AddedQuantity = discountMultiplier * promo.DiscountValue;
                break;
            default: 
                break;
        }

        return da;
    }
  }
}