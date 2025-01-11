using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IRatingService
    {
        Task<bool> CanUserRateDishAsync(Guid userId, Guid dishId);
        Task SetRatingAsync(Guid userId, Guid dishId, int score);
    }
}
