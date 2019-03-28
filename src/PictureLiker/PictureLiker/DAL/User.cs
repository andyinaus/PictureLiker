using System;
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

        public User SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));

            try
            {
                var emailAddress = new System.Net.Mail.MailAddress(email);
                Email = emailAddress.Address;

                return this;
            }
            catch
            {
                throw new ArgumentException("Invalid email address.", nameof(email));
            }
        }
    }
}
