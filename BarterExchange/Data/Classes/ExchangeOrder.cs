using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BarterExchange.Data.Classes
{
    public class ExchangeOrder
    {
        [BsonIgnoreIfDefault]
        public ObjectId _id;
        public int ExchangeOrderId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ExchangeDescription { get; set; }
        public int ItemTypeId { get; set; }

        public ExchangeOrder(string title, string description, string exchangeDescription, int itemTypeId) 
        {
            Title = title;
            Description = description;
            ExchangeDescription = exchangeDescription;
            ItemTypeId = itemTypeId;
        }
    }
}
