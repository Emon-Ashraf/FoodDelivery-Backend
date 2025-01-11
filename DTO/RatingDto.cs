using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class RatingDto
    {
        public Guid DishId { get; set; }
        public int Score { get; set; }
    }
}
