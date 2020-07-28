using API.Dtos;
using API.Entities;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
  public class ImageUrlResolver : IValueResolver<Package, PackageDto, string>
  {
    private readonly IConfiguration _config;
    public ImageUrlResolver(IConfiguration config)
    {
      _config = config;
    }

    public string Resolve(Package source, PackageDto destination, string destMember, ResolutionContext context)
    {
        if(!string.IsNullOrEmpty(source.ImgURL))
        {
            return _config["ApiUrl"] + source.ImgURL;
        }

        return null;
    }
  }
}