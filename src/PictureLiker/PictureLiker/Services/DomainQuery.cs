using System;
using System.Threading.Tasks;
using PictureLiker.DAL;
using PictureLiker.Extensions;

namespace PictureLiker.Services
{
    public class DomainQuery : IDomainQuery 
    {
        private readonly IUnitOfWork _unitOfWork;

        public DomainQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> IsEmailInUse(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));

            return await _unitOfWork.UseRepository.FirstOrDefaultAsync(u => u.Email.EqualsIgnoreCase(email)) != null;
        }
    }
}
