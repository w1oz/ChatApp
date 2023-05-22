using MongoDB.Bson.Serialization.Attributes;

namespace ChatApp.Model
{
    public class UserLogin
    {
        [BsonRequired]
        public string? Username { get; set; }
        [BsonRequired]
        public string? Password { get; set; }
    }
}
