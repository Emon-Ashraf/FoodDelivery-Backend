using DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IDishService
    {
        Task<DishPagedListDto> GetDishesAsync(List<string> categories, bool? vegetarian, string sorting, int page);
        Task<DishDto> GetDishByIdAsync(Guid id);
    }
}
