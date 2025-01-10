using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO;

namespace BLL.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderInfoDto>> GetOrdersAsync(Guid userId);
        Task<OrderDto> GetOrderByIdAsync(Guid userId, Guid orderId);
        Task<OrderDto> CreateOrderAsync(Guid userId, OrderCreateDto orderCreateDto);
        Task ConfirmOrderDeliveryAsync(Guid userId, Guid orderId);
    }
}
