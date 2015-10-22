using System.Threading.Tasks;

namespace Store.WebUI.Infrastructure.Abstract
{
    public interface IAuthProvider
    {
        Task<bool> Authenticate(string username, string password);
    }
}

