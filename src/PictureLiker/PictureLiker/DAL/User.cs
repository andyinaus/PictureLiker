using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using PictureLiker.DAL.Repositories;
using PictureLiker.Exceptioons;
using PictureLiker.Extensions;
using PictureLiker.Services;

namespace PictureLiker.DAL
{
    [Table("Users")]
    public class User : EntityBase
    {
        private readonly IDomainQuery _domainQuery;

        private User() { }

        public User(IDomainQuery domainQuery)
        {
            _domainQuery = domainQuery ?? throw new ArgumentNullException(nameof(domainQuery));
        }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; private set; }

        [DataType(DataType.Text)]
        [Required]
        public string Name { get; private set; }

        [DataType(DataType.Text)]
        [Required]
        public string Role { get; private set; }

        public async Task<User> SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));

            try
            {
                new System.Net.Mail.MailAddress(email);
            }
            catch
            {
                throw new ArgumentException("Invalid email address.", nameof(email));
            }

            if (await _domainQuery.IsEmailInUse(email)) throw new EmailIsAlreadyInUseException(email);
            
            Email = email;

            return this;
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

            if (!role.EqualsIgnoreCase(Authentication.RoleTypes.GeneralUser) &&
                !role.EqualsIgnoreCase(Authentication.RoleTypes.Administrator))
            {
                throw new ArgumentException("Invalid user role.", nameof(role));
            }

            Role = role;

            return this;
        }
    }
}
