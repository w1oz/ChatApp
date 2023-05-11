using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ChatApp.Model
{
    public class GroupModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement]
        public Array UserID { get; set; }

    }
}
