using System.ComponentModel.DataAnnotations;

namespace LearnAspNetCoreMVC.Models
{
    public class Account
    {
        [Key]
        [MaxLength(50)]
        [Required(ErrorMessage = "Username field can't be empty!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password field can't be empty!")]
        public string Password { get; set; }

    }
}
