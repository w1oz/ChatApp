using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ChatApp.Model
{
    public class FriendModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement]
        public string UserID { get; set; }

        [BsonElement]
        public string FriendID { get; set; }

    }
}
