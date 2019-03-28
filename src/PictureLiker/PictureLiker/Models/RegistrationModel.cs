using System.ComponentModel.DataAnnotations;

namespace PictureLiker.Models
{
    public class RegistrationModel
    {
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        [Required]
        public string PreferredName { get; set; }
    }
}
