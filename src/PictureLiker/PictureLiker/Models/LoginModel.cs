using System.ComponentModel.DataAnnotations;

namespace PictureLiker.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PreferredName { get; set; }
    }
}
