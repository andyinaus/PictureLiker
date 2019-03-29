using System;
using System.Threading.Tasks;
using PictureLiker.DAL;
using PictureLiker.DAL.Repositories;
using PictureLiker.Extensions;

namespace PictureLiker.Services
{
    public class DomainQuery : IDomainQuery 
    {
        private readonly IRepository<User> _useRepository;

        public DomainQuery(IRepository<User> useRepository)
        {
            _useRepository = useRepository ?? throw new ArgumentNullException(nameof(useRepository));
        }

        public async Task<bool> IsEmailInUse(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));

            return await _useRepository.FirstOrDefaultAsync(u => u.Email.EqualsIgnoreCase(email)) != null;
        }
    }
}
