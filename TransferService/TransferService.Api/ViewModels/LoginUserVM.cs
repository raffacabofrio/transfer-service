﻿using System.ComponentModel.DataAnnotations;

namespace TransferService.Api.ViewModels
{
    public class LoginUserVM
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
