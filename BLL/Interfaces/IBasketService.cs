using DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IBasketService
    {
        Task<List<DishBasketDto>> GetBasketAsync(Guid userId);
        Task AddDishToBasketAsync(Guid userId, Guid dishId);
        Task RemoveDishFromBasketAsync(Guid userId, Guid dishId, bool increase);
    }
}
