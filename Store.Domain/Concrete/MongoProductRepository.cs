using MongoDB.Bson;
using MongoDB.Driver;
using Store.Domain.Abstract;
using Store.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Domain.Concrete
{
    public class MongoProductRepository : IProductRepository
    {
        private static MongoClient client = new MongoClient();
        private static IMongoDatabase db = client.GetDatabase("storeDB");
        private static SortDefinition<Product> order = Builders<Product>.Sort.Descending("_id");

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var collection = await db.GetCollection<Product>("product").Find(new BsonDocument()).Sort(order).ToListAsync();
            return collection;
        }

        public async Task<Product> DeleteProductAsync(string productID)
        {
            IMongoCollection<Product> collection = db.GetCollection<Product>("product");

            var filter = new BsonDocument("_id", new ObjectId(productID));
            var forDelete = await collection.FindOneAndDeleteAsync(filter);

            return forDelete;
        }

        public async Task<bool> SaveProductAsync(Product product)
        {
            IMongoCollection<Product> collection = db.GetCollection<Product>("product");

            if (product.Id == ObjectId.Empty)
            {
                await collection.InsertOneAsync(product);
            }
            else
            {
                var filter = new BsonDocument("_id", product.Id);

                var dbEntry = await db.GetCollection<Product>("product").Find(filter).FirstOrDefaultAsync();

                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;
                    dbEntry.ImageDataBig = product.ImageDataBig;
                    dbEntry.ImageDataSmall = product.ImageDataSmall;
                    dbEntry.ImageMimeType = product.ImageMimeType;

                    await collection.ReplaceOneAsync(filter, dbEntry);
                }
                else
                    return false;
                
            }

            return true;
        }


        public async Task<Product> GetProductById(string productId)
        {
            var filter = new BsonDocument("_id", new ObjectId(productId));

            var product = await db.GetCollection<Product>("product").Find(filter).ToListAsync();

            return product.FirstOrDefault();
        }

        public async Task<IEnumerable<string>> GetCategories()
        {

            FieldDefinition<BsonDocument, string> field = "category";
            FilterDefinition<BsonDocument> filter = new BsonDocument();
            var products = db.GetCollection<BsonDocument>("product");

            var categories = await products.DistinctAsync(field, filter).GetAwaiter().GetResult().ToListAsync();

            categories.Sort();

            return categories;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(string category)
        {
            var filter = new BsonDocument();

            if (category!= null)
            {
                filter = new BsonDocument("category", category);
            }
                   
            var product = await db.GetCollection<Product>("product").Find(filter).Sort(order).ToListAsync();

            return product;
        }
    }
}
