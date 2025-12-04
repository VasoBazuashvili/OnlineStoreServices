using AutoMapper;
using OrdersService.Application.DTOs;
using OrdersService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Application.Mapping
{
	public class OrderMappingProfile : Profile
	{
		public OrderMappingProfile()
		{
			CreateMap<OrderItem, OrderItemDto>()
				.ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
				.ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName));

			CreateMap<Order, OrderDto>()
				.ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
		}
	}
}
