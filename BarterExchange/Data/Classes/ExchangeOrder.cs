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
        public string ContactInformation { get; set; }
        public int ItemTypeId { get; set; }
        public string PhotoName { get; set; }
        public string CreatorEmail { get; set; }
        public bool IsConducted { get; set; }

        public ExchangeOrder(string title, string description, string exchangeDescription, string contactInformation,
        int itemTypeId, string photoName, string creatorEmail) 
        {
            Title = title;
            Description = description;
            ExchangeDescription = exchangeDescription;
            ContactInformation = contactInformation;
            ItemTypeId = itemTypeId;
            PhotoName = photoName;
            CreatorEmail = creatorEmail;
        }
    }
}
