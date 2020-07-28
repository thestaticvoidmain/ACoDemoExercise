using API.Dtos;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
  public class MappingProfiles : Profile
  {
    public MappingProfiles()
    {
        CreateMap<Package, PackageDto>()
            .ForMember(d => d.ImgURL, o => o.MapFrom<ImageUrlResolver>());
    }
  }
}