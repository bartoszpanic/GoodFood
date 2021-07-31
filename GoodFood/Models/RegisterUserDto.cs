﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoodFood.Models
{
    public class RegisterUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        public string Nationality { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public int RoleId { get; set; } = 1;
    }
}
