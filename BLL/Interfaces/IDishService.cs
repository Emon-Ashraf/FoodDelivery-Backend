using DTO;
using DTO.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IDishService
    {
        Task<DishPagedListDto> GetDishesAsync(List<string> categories, bool? vegetarian, DishSorting sorting, int page);
        Task<DishDto> GetDishByIdAsync(Guid id);
    }
}
