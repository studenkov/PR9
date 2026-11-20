using AutoMapper;
using PR9.Dtos;
using PR9.Models;
using PR9.Services;

namespace PR9.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<CreateProductDto, Product>().ForMember(d => d.Category, o => o.Ignore());
        CreateMap<UpdateProductDto, Product>().ForMember(d => d.Category, o => o.Ignore());
        CreateMap<Category, CategoryDto>();
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDTO, Category>();
        CreateMap<Category, CategoryDTO>();
        CreateMap<Role, RoleDTO>();
        CreateMap<CreateRoleDTO, Role>();
        CreateMap<UpdateRoleDTO, Role>();
        CreateMap<AppUser, UserDTO>()
            .ForMember(d => d.Login, o => o.MapFrom(s => s.Login))
            .ForMember(d => d.LoginUser, o => o.MapFrom(s => s.Login));
        CreateMap<CreateUserDTO, AppUser>()
            .ForMember(d => d.Login, o => o.MapFrom(s => s.LoginUser));
        CreateMap<UpdateUserDTO, AppUser>()
            .ForMember(d => d.Login, o => o.MapFrom(s => s.LoginUser));
        CreateMap<Cart, CartDTO>();
        CreateMap<CreateCartDTO, Cart>();
        CreateMap<UpdateCartDTO, Cart>();
        CreateMap<CartItem, CartItemDTO>();
        CreateMap<CreateCartItemDTO, CartItem>();
        CreateMap<UpdateCartItemDTO, CartItem>();
        CreateMap<Order, OrderDTO>();
        CreateMap<CreateOrderDTO, Order>();
        CreateMap<UpdateOrderDTO, Order>();
        CreateMap<OrderItem, OrderItemDTO>();
        CreateMap<CreateOrderItemDTO, OrderItem>();
        CreateMap<UpdateOrderItemDTO, OrderItem>();
        CreateMap<ProductReservation, ProductReservationDto>();
    }
}
