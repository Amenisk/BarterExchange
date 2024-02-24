using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Formats.Asn1;
using BarterExchange.Data.Services;
using MongoDB.Driver;

namespace BarterExchange.Data.Classes
{
    public class Target
    {
        [BsonIgnoreIfDefault]
        public ObjectId _id;
        public int TargetId { get; set; }
        public ExchangeOrder StartOrder { get; set; }
        public string TargetNameItem { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CreatorEmail { get; set; }
        public List<List<int>> TargetLevels { get; set; }
        public List<int> AvailableOrdersIdList { get; set; }

        public Target(ExchangeOrder startOrder, string targetNameItem)
        {
            TargetId = Database.GetLastTargetId() + 1;
            StartOrder = startOrder;
            TargetNameItem = targetNameItem;
            StartDate = DateTime.Now;
            TargetLevels = new List<List<int>>()
            { 
                new List<int>() { }
            };
            CreatorEmail = startOrder.CreatorEmail;
            AvailableOrdersIdList = new List<int>
            {
                startOrder.ExchangeOrderId
            };
        }

        public void AddLevel(ExchangeOrderOffer offer)
        {
            bool isContains = true; 

            foreach(var orderId in offer.SenderExchangeOrdersId)
            {
                if(!AvailableOrdersIdList.Contains(orderId))
                {
                    isContains = false; 
                    break;
                }
            }

            if(isContains)
            {
                foreach (var orderId in offer.SenderExchangeOrdersId)
                {
                    if (AvailableOrdersIdList.Contains(orderId))
                    {
                        AvailableOrdersIdList.Remove(orderId);
                    }
                }
            } 

            TargetLevels.Last().Add(offer.ExchangeOfferId);

            if (AvailableOrdersIdList.Count == 0)
            {
                foreach(var offerId in TargetLevels.Last())
                {
                    AvailableOrdersIdList.AddRange(Database.GetExchangeOrderOfferById(offerId).RecipientExchangeOrdersId);
                }

                TargetLevels.Add(new List<int>());
                return;
            }
        }

        public void RemoveOfferFromLastLevel(ExchangeOrderOffer offer)
        {
            TargetLevels.Last().Remove(offer.ExchangeOfferId);
            RemoveOrdersId(offer);
        }

        public bool CheckButOneLevel(List<int> level)
        {
            return TargetLevels.Last().Count() == 0 && level == GetLastButOneLevel();
        }
        public void RemoveOfferFromButOneLevel(ExchangeOrderOffer offer)
        {
            List<int> level = new List<int>();

            foreach (var l in TargetLevels)
            {
                if (l != TargetLevels.Last())
                {
                    level = l;
                }
            }

            level.Remove(offer.ExchangeOfferId);
            RemoveOrdersId(offer);
        }

        private void RemoveOrdersId(ExchangeOrderOffer offer)
        { 
            foreach(var id in offer.RecipientExchangeOrdersId)
            {
                AvailableOrdersIdList.Remove(id);
            }

            if(AvailableOrdersIdList.Count == 0)
            {
                TargetLevels.Remove(TargetLevels.Last());

                if(TargetLevels.First() != TargetLevels.Last())
                {
                    foreach (var offId in GetLastButOneLevel())
                    {
                        foreach (var orderId in Database.GetOfferById(offId).RecipientExchangeOrdersId)
                        {
                            AvailableOrdersIdList.Add(orderId);
                        }
                    }
                }
                else
                {
                    AvailableOrdersIdList.Add(StartOrder.ExchangeOrderId);
                }

                return;
            }

            foreach (var id in offer.SenderExchangeOrdersId)
            {
                AvailableOrdersIdList.Add(id);
            }
        }

        private List<int> GetLastButOneLevel()
        {
            List<int> level = new List<int>();

            foreach (var l in TargetLevels)
            {
                if (l != TargetLevels.Last())
                {
                    level = l;
                }
            }

            return level;
        }

        

    }
}
