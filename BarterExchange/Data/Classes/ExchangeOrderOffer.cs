using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BarterExchange.Data.Classes
{
    public class ExchangeOrderOffer
    {
        [BsonIgnoreIfDefault]
        public ObjectId _id;
        public int ExchangeOfferId { get; set; }
        public List<int> SenderExchangeOrdersId { get; set; }
        public List<int> RecipientExchangeOrdersId { get; set; }
        public string SenderEmail { get; set; }
        public string RecipientEmail { get; set; }
        public bool IsConducted { get; set; }

        public ExchangeOrderOffer(List<int> senderExchangeOrdersId, List<int> recipientExchangeOrdersId) 
        { 
            SenderExchangeOrdersId = senderExchangeOrdersId;
            RecipientExchangeOrdersId = recipientExchangeOrdersId;
            SenderEmail = Database.GetExchangeOrderById(senderExchangeOrdersId.First()).CreatorEmail;
            RecipientEmail = Database.GetExchangeOrderById(recipientExchangeOrdersId.First()).CreatorEmail;
            ExchangeOfferId = Database.GetLastExchangeOfferId() + 1;
        }
    }
}
