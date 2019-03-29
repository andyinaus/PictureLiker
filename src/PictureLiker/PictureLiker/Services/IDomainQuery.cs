using System.Threading.Tasks;

namespace PictureLiker.Services
{
    public interface IDomainQuery
    {
        Task<bool> IsEmailInUse(string email);
    }
}
