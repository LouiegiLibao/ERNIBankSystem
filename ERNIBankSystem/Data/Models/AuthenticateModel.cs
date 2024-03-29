﻿using System.ComponentModel.DataAnnotations;

namespace ERNIBankSystem.Data.Models
{
    public class AuthenticateModel
    {
        [Required]
        public string AccountNumber { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Pin must be 4-digit")]
        public string Pin { get; set; }
    }
}
