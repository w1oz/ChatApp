using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ChatApp.Model
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement]
        public string FirstName { get; set; }

        [BsonElement]
        public string LastName { get; set; }

        [BsonElement]
        public string UserName { get; set; }

        [BsonElement]
        public string Password { get; set; }

        [BsonElement]
        public string RefreshToken { get; set; }
    }
}
