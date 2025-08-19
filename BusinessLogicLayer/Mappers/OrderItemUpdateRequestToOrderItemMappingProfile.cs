using AutoMapper;
using eCommerce.OrdersMicroservice.BusinessLogicLayer.DTO;
using eCommerce.OrdersMicroservice.DataAccessLayer.Entities;

namespace eCommerce.ordersMicroservice.BusinessLogicLayer.Mappers;

public class OrderItemUpdateRequestToOrderItemMappingProfile : Profile
{
  public OrderItemUpdateRequestToOrderItemMappingProfile()
  {
    CreateMap<OrderItemUpdateRequest, OrderItem>()
       //.ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductID))
       //.ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
       //.ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
       //.ForMember(dest => dest.TotalPrice, opt => opt.Ignore())
       //.ForMember(dest => dest.OrderItemID, opt => opt.Ignore());
       .ForMember(dest => dest.OrderItemID, opt => opt.MapFrom(src => src.OrderItemID)) // ✅ map ID
            .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductID))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.TotalPrice, opt => opt.Ignore()) // set in service
            .ForMember(dest => dest.OrderID, opt => opt.Ignore());
    }
}