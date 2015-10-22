using System.Threading.Tasks;

namespace Store.Domain.Abstract
{
    public interface IUserRepository
    {
        Task<string> GetCredentails(string username);
    }
}
