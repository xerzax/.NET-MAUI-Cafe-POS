using BisleriumCafeCourseWork.Data.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BisleriumCafeCourseWork.Data
{
    //Model representing trhe users of the system
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "Username is required")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long")]
        public string Username { get; set; }
		public string SpecialPassword { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; }
		public string PasswordHash { get; set; }
        public Role Role { get; set; }
        public bool HasInitialPassword { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid CreatedBy { get; set; }
    }
}
