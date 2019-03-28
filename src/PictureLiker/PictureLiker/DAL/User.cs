using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PictureLiker.DAL.Repositories;

namespace PictureLiker.DAL
{
    [Table("Users")]
    public class User : EntityBase
    {
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; private set; }
    }
}
