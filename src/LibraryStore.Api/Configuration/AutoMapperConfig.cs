using AutoMapper;
using LibraryStore.Api.Dtos;
using LibraryStore.Models;

namespace LibraryStore.Api.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Provider, ProviderDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<ProductDto, Product>();

            //CreateMap<ProdutoImagemViewModel, Produto>().ReverseMap();

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProviderName, opt => opt.MapFrom(src => src.Provider.Name));
        }
    }
}
