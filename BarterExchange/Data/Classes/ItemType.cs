using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BarterExchange.Data.Classes
{
    public class ItemType
    {
        [BsonIgnoreIfDefault]
        public ObjectId _id;
        public int ItemTypeId { get; set; }
        public string Title { get; set; }

    }
}
