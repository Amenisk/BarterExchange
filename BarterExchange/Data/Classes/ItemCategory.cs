using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BarterExchange.Data.Classes
{
    public class ItemCategory
    {
        [BsonIgnoreIfDefault]
        public ObjectId _id;
        public int ItemClassId { get; set; }
        public string Title { get; set; }

        public ItemCategory(string title) 
        {
            Title = title;
        }

    }
}
