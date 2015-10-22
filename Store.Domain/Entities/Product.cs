using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Store.Domain.Entities
{
    public class Product
    {
        public ObjectId Id { get; set; }

        public string IdString
        {
            get
            {
                return Id.ToString();
            }
            set
            {
                Id = new ObjectId(value);
            }
        }

        [BsonElement("name")]
        [Required(ErrorMessage = "Please enter a product name")]
        public string Name { get; set; }

        [BsonElement("decription")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Please enter a description")]
        public string Description { get; set; }

        [BsonElement("price")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive price")]
        public decimal Price { get; set; }

        [BsonElement("category")]
        [Required(ErrorMessage = "Please specify a category")]
        public string Category { get; set; }

        public byte[] ImageDataSmall { get; set; }
        public byte[] ImageDataBig { get; set; }
        public string ImageMimeType { get; set; }
    }
}
