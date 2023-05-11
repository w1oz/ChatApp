using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text;

namespace ChatApp.Model
{
    public class MessageModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement]
        public int SenderID { get; set; }

        [BsonElement]
        public int RoomId { get; set; }

        [BsonElement]
        [BsonRequired]
        public string Content { get; set; }

        [BsonElement]
        public long TimeStamp { get; set; }


    }
}
