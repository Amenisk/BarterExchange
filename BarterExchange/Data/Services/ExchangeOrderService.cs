using BarterExchange.Data.Classes;

namespace BarterExchange.Data.Services
{
    public class ExchangeOrderService
    {
        public ExchangeOrder CurrentExchangeOrder { get; private set; }
        public int ValueItemType { get; set; }

        public bool IsSenderOrders { get; set; }    

        public List<ExchangeOrder> SenderOrders { get; set; } = new List<ExchangeOrder>();

        public List<ExchangeOrder> RecepientOrders { get; set; } = new List<ExchangeOrder>();

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

        public void SaveExchageOrderOffer()
        {
            Database.SaveExchangeOrderOffer(new ExchangeOrderOffer(GetOrdersId(SenderOrders), GetOrdersId(RecepientOrders)));
        }

        public bool CheckExchangeOrderOffer()
        {
            return Database.CheckExchangeOrderOffer(new ExchangeOrderOffer(GetOrdersId(SenderOrders), GetOrdersId(RecepientOrders)));
        }

        public List<ExchangeOrderOffer> GetCreatedExchangeOrderOffers(string senderEmail)
        {
            return Database.GetExchangeOffersBySenderEmail(senderEmail);
        }
        public List<ExchangeOrderOffer> GetRecievedExchangeOrderOffers(string recipientEmail)
        {
            return Database.GetExchangeOffersByRecipientEmail(recipientEmail);
        }

        public void AcceptExchangeOffer(ExchangeOrderOffer offer)
        {
            Database.AcceptExchangeOffer(offer);
        }

        public List<ExchangeOrderOffer> GetConductedExchangeOrderOffers(string email)
        {
            return Database.GetAllConductedOffersByUserEmail(email);
        }

        public List<ExchangeOrder> GetListExchangeOrdersBySearch(string searchText) 
        {
            return Database.SearchByTitleCategoryAndTypeItem(searchText);
        }

        public List<ExchangeOrder> GetRecomendedOrders(string email)
        {
            return Database.GetRecomendedOrdersByUserEmail(email);
        }

        public bool CheckAvailabilityExchangeOrders(string email)
        {
            return Database.CheckExchangeOrdersByUserEmail(email);
        }

        public List<List<ExchangeOrder>> GetRelevantExchnageOrders(string email, int value)
        {
            return Database.GetRelevantExchangeOrdersByRecommendation(email, value);    
        }

        public void RejectExchangeOffer(int offerId)
        {
            Database.RejectExchangeOffer(offerId);
        }

        public bool CheckAvailabilityOrderInLists()
        {
            foreach(var o in RecepientOrders) 
            { 
                if(o.ExchangeOrderId == CurrentExchangeOrder.ExchangeOrderId)
                {
                    return true;
                }
            }

            return false;
        }

        public void ReloadLists(ExchangeOrder order)
        {
            SenderOrders.Clear();
            RecepientOrders.Clear();
            RecepientOrders.Add(order);
        }

        public bool CheckFullnessList()
        {
            return SenderOrders.Count() > 0 && RecepientOrders.Count() > 0;
        }

        private List<int> GetOrdersId(List<ExchangeOrder> list)
        {
            List<int> newList = new List<int>();

            foreach(var order in list) 
            { 
                newList.Add(order.ExchangeOrderId);
            }

            newList.Sort();
            return newList;
        }

        public void ClearLists()
        {
            SenderOrders.Clear();
            RecepientOrders.Clear();
        }
    }
}
