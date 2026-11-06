using AutoMapper;
using PR9.DTOs;
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
        CreateMap<CreateCategoryDTO, Category>();
        CreateMap<Category, CategoryDTO>();
        CreateMap<Role, RoleDTO>();
        CreateMap<CreateRoleDTO, Role>();
        CreateMap<UpdateRoleDTO, Role>();
        CreateMap<User, UserDTO>();
        CreateMap<CreateUserDTO, User>();
        CreateMap<UpdateUserDTO, User>();
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
