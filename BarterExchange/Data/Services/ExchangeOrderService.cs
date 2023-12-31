﻿using BarterExchange.Data.Classes;

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

        public ExchangeOrder GetExchangeOrder(int id)
        {
            return Database.GetExchangeOrderById(id);
        }

        public void ChangeCurrentExchangeOrder(int id)
        {
            CurrentExchangeOrder = GetExchangeOrder(id);
        }
    }
}
