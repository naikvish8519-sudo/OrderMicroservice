﻿using AutoMapper;
using eCommerce.OrdersMicroservice.BusinessLogicLayer.DTO;
using eCommerce.OrdersMicroservice.DataAccessLayer.Entities;

namespace eCommerce.ordersMicroservice.BusinessLogicLayer.Mappers;

public class OrderAddRequestToOrderMappingProfile : Profile
{
  public OrderAddRequestToOrderMappingProfile()
  {
    CreateMap<OrderAddRequest, Order>()
      .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.UserId))
      .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
      .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
      .ForMember(dest => dest.OrderID, opt => opt.Ignore())
      //.ForMember(dest => dest.Order, opt => opt.Ignore())
      .ForMember(dest => dest.TotalBill, opt => opt.Ignore());
  }
}