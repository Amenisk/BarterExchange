using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BarterExchange.Data.Classes
{
    public class ExchangeOrderOffer
    {
        [BsonIgnoreIfDefault]
        public ObjectId _id;
        public int SenderExchangeOrderId { get; set; }
        public int RecipientExchangeOrderId { get; set; }
        public string SenderEmail { get; set; }
        public string RecipientEmail { get; set; }
        public bool IsConducted { get; set; }

        public ExchangeOrderOffer(int senderExchangeOrderId, int recipientExchangeOrderId) 
        { 
            SenderExchangeOrderId = senderExchangeOrderId;
            RecipientExchangeOrderId = recipientExchangeOrderId;
            SenderEmail = Database.GetExchangeOrderById(senderExchangeOrderId).CreatorEmail;
            RecipientEmail = Database.GetExchangeOrderById(recipientExchangeOrderId).CreatorEmail;
        }
    }
}
