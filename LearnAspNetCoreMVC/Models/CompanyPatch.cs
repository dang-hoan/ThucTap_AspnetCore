﻿using System.ComponentModel.DataAnnotations;

namespace LearnAspNetCoreMVC.Models
{
    public class CompanyPatch
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Company name field can't be empty!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Company address field can't be empty!")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Company phone field can't be empty!")]
        [RegularExpression(@"(\+84|84|0)+(3|5|7|8|9|1[2|6|8|9])+([0-9]{8})\b",
            ErrorMessage = "Please enter a valid phone number")]
        public string PhoneNumber { get; set; }
    }
}