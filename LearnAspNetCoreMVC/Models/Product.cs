﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnAspNetCoreMVC.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [DisplayName("Display order")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1 and 100 only!!")]
        public int DisplayOrder { get; set; }

        [ForeignKey("Company")]
        public int CompanyID { get; set; }
        public Company? Company { get; set; }
        public DateTime? CreateDate { get; set; } = null;
        public DateTime? UpdateDate { get; set; } = null;
        public DateTime? DeleteDate { get; set; } = null;
        public bool IsDelete { get; set; } = false;
    }
}
