using APIRestProductManagement.Dtos;
using APIRestProductManagement.Entities;
using AutoMapper;

namespace APIRestProductManagement.Utils
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<DisableEnableProductoDto, Product>().ReverseMap();
        }
    }
}
