using System.ComponentModel.DataAnnotations;

namespace HouseOfSoulSounds.Models.Identity
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Введите Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
