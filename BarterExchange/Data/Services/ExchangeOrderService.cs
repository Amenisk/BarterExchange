using BarterExchange.Data.Classes;

namespace BarterExchange.Data.Services
{
    public class ExchangeOrderService
    {
        public ExchangeOrder CurrentExchangeOrder { get; private set; }

        public int GetLastId()
        {
            var exchangeOrder = Database.GetLastExchangeOrder();

            if(exchangeOrder == null)
            {
                return 0;
            }
            else
            {
                return exchangeOrder.ExchangeOrderId;
            }
        }

        public void SaveExchangeOrder(ExchangeOrder exchangeOrder)
        {
            exchangeOrder.ExchangeOrderId = GetLastId() + 1;
            Database.SaveExchangeOrder(exchangeOrder);
        }

        public int GetItemTypeIdByTitle(string title)
        {
            var itemType = Database.GetItemTypeByTitle(title);

            return itemType.ItemTypeId;
        }

        public List<ExchangeOrder> GetAllExchangeOrders() 
        {
            return Database.GetAllExchangeOrders(); 
        }

        public List<ExchangeOrder> GetCreatedExchangeOrders(string email)
        {
            return Database.GetExchangeOrdersByCreatorEmailAndConduct(email, false);
        }

        public List<ExchangeOrder> GetConductedExchangeOrders(string email)
        {
            return Database.GetExchangeOrdersByCreatorEmailAndConduct(email, true);
        }

        public ExchangeOrder GetExchangeOrder(int id)
        {
            return Database.GetExchangeOrderById(id);
        }

        public void ChangeCurrentExchangeOrder(int id)
        {
            CurrentExchangeOrder = GetExchangeOrder(id);
        }

        public void ConductExchangeOrder(int id)
        {
            Database.ConductExchangeOrder(id);
        }

        public void DeleteExchangeOrder(int id)
        {
            CurrentExchangeOrder = null;
            Database.DeleteExchangeOrder(id);
        }

        public void EditOrder(ExchangeOrder order)
        {
            Database.ReplaceExchangeOrder(order);   
        }

        public void SaveExchageOrderOffer(ExchangeOrderOffer offer)
        {
            Database.SaveExchangeOrderOffer(offer);
        }

        public bool CheckExchangeOrderOffer(int senderOfferId, int recepientOrderId)
        {
            return Database.GetExchangeOrderOfferByTwoId(senderOfferId, recepientOrderId) != null;  
        }

        public List<ExchangeOrderOffer> GetCreatedExchangeOrderOffers(string senderEmail)
        {
            return Database.GetExchangeOffersBySenderEmail(senderEmail);
        }
        public List<ExchangeOrderOffer> GetRecievedExchangeOrderOffers(string recipientEmail)
        {
            return Database.GetExchangeOffersByRecipientEmail(recipientEmail);
        }

        public void AcceptExchangeOffer(int senderOrderId, int recepientOrderId)
        {
            Database.AcceptExchangeOffer(senderOrderId, recepientOrderId);
        }
    }
}
