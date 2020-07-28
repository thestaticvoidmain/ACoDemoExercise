using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class PackagesController : ControllerBase
  {
    private readonly StoreContext _context;
    private readonly IMapper _mapper;

    public PackagesController(StoreContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<Package>>> GetPackagesAsync()
    {
      var pkgs = await _context.Packages.ToListAsync();

      return Ok(_mapper.Map<List<Package>, List<PackageDto>>(pkgs));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Package>> GetPackageAsync(int id)
    {
      return await _context.Packages.FindAsync(id);
    }

    [HttpGet("promos")]
    public async Task<ActionResult<IReadOnlyList<Promo>>> GetPromosAsync()
    {
      return await _context.Promos.Include(x => x.Package).Include(x => x.Discount).ToListAsync();
    }

    [HttpGet("discounts")]
    public async Task<ActionResult<IReadOnlyList<Discount>>> GetDiscountsAsync()
    {
      return await _context.Discounts.Include(x => x.DiscountType).ToListAsync();
    }
  }
}