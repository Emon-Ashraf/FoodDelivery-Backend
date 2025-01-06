using DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class DishFilterModel
    {
        public List<DishCategory> Categories { get; set; } = new List<DishCategory>();
        public bool? Vegetarian { get; set; }
        public DishSorting Sorting { get; set; } = DishSorting.NameAsc;
        public int Page { get; set; } = 1;
    }
}
