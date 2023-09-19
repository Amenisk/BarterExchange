using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BarterExchange.Data.Classes
{
    public class ItemClass
    {
        [BsonIgnoreIfDefault]
        public ObjectId _id;
        public int ItemClassId { get; set; }
        public string Title { get; set; }

        public ItemClass(string title) 
        {
            Title = title;
        }

    }
}
