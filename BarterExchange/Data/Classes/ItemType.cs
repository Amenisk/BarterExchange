using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BarterExchange.Data.Classes
{
    public class ItemType
    {
        [BsonIgnoreIfDefault]
        public ObjectId _id;
        public int ItemTypeId { get; set; }
        public int ItemCategoryId { get; set; }
        public string Title { get; set; }
        public int Value { get; set; }

        public ItemType(int itemCategoryId, string title, int value)
        {
            ItemCategoryId = itemCategoryId;
            Title = title;
            Value = value;
        }
    }
}
