using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Entities
{
    public class User
    {
        public ObjectId id { get; set; }

        [BsonElement("username")]
        [Required]
        public String Username { get; set; }
        [BsonElement("password")]
        [Required]
        public String Password { get; set; }
    }
}
