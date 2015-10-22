using Store.Domain.Abstract;
using Store.WebUI.Infrastructure.Abstract;
using System.Threading.Tasks;
using System.Web.Security;

namespace Store.WebUI.Infrastructure.Concrete
{
    public class BcryptAuthProvider : IAuthProvider
    {
        private static IUserRepository repository;

        public BcryptAuthProvider(IUserRepository userRepository)
        {
            repository = userRepository;
        }

         public async Task<bool> Authenticate(string username, string password)
        {
            bool result = false;

            string hash = await repository.GetCredentails(username);
            if(hash!=null)
                result = BCrypt.Net.BCrypt.Verify(password, hash);
            
            if(result)
                FormsAuthentication.SetAuthCookie(username, false);

            return result;
        }
    }
}