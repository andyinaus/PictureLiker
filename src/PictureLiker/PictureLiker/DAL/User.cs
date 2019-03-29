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

        [DataType(DataType.Text)]
        [Required]
        public string Name { get; private set; }

        [DataType(DataType.Text)]
        [Required]
        public string Role { get; private set; }

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

        public User SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            Name = name;

            return this;
        }

        public User SetRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role)) throw new ArgumentNullException(nameof(role));

            if (!role.Equals(Authentication.RoleTypes.GeneralUser, StringComparison.InvariantCultureIgnoreCase) &&
                !role.Equals(Authentication.RoleTypes.Administrator, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException("Invalid user role.", nameof(role));
            }

            Role = role;

            return this;
        }
    }
}
