using DTO.Enums;
using System;

namespace DTO
{
    public class UserEditModel
    {
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
