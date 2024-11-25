using AutoMapper;
using DAL.Concrete;
using TradingCompany_WEB.Models;

namespace TradingCompany_WEB.App.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductModel, ProductDto>();
            CreateMap<ProductDto, ProductModel>();
        }
    }
}
