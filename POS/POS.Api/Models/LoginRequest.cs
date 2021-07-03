using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace POS.Api.Models
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(60)]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
