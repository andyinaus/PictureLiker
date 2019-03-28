using System.ComponentModel.DataAnnotations;
using PictureLiker.DAL.Repositories;

namespace PictureLiker.DAL
{
    public class User : EntityBase
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; private set; }
    }
}
