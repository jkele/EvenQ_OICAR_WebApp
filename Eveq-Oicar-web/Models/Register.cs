using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eveq_Oicar_web.Models
{
    public class Register
    {
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be 2 characters or longer")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be 2 characters or longer")]
        public string LastName { get; set; }
        
        [Required]
        [Range(18, int.MaxValue, ErrorMessage = "Sorry no can't do, please enter age between 18 and higher")]
        public int Age { get; set; }
        [Required]
        public string ReferralCode { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
