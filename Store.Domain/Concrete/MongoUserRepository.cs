using MongoDB.Bson;
using MongoDB.Driver;
using Store.Domain.Abstract;
using Store.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Domain.Concrete
{
    public class MongoUserRepository : IUserRepository
    {
        private static MongoClient client = new MongoClient();
        private static IMongoDatabase db = client.GetDatabase("storeDB");

        public async Task<string> GetCredentails(string username)
        {
            var filter = new BsonDocument("username", username);

            var user = await db.GetCollection<User>("user").Find(filter).ToListAsync();

            if(user.Count!=0)
                return user.FirstOrDefault().Password;
            return null;
        }
    }
}
