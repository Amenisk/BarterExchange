﻿using BarterExchange.Data.Classes;
using System;

namespace BarterExchange.Data.Services
{
    public class ExchangeOrderService
    {
        public ExchangeOrder CurrentExchangeOrder { get; private set; }
        public int ValueItemType { get; set; }
        public bool IsSenderOrders { get; set; }    
        public bool IsTargetOrder { get; set; }
        public Target CurrentTarget { get; set; }   
        public ExchangeOrderOffer SelectedOffer { get; set; }
        public List<ExchangeOrder> SenderOrders { get; set; } = new List<ExchangeOrder>();
        public List<ExchangeOrder> RecepientOrders { get; set; } = new List<ExchangeOrder>();
        public List<ExchangeOrder> FilterOrders { get; set; } = new List<ExchangeOrder>();

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
            return Database.GetExchangeOrdersByCreatorEmail(email);
        }

        public ExchangeOrder GetExchangeOrder(int id)
        {
            return Database.GetExchangeOrderById(id);
        }

        public void ChangeCurrentExchangeOrder(int id)
        {
            CurrentExchangeOrder = GetExchangeOrder(id);
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
            FilterOrders.Clear();
        }

        public string CutName(string name)
        {
            if(name.Length <= 25)
            {
                return name;
            }
            else
            {
                return name.Substring(0, 22) + "...";
            }
        }

        public List<Target> GetAllTargetsByUserEmail(string userEmail)
        {
            return Database.GetAllTargetsByUserEmail(userEmail);
        }

        public void SaveTarget(Target target)
        {
            Database.SaveTarget(target);
        }

        public void ChangeTargetItemName(int targetId,  string newName)
        {
            Database.ChangeTargetItemName(targetId, newName);
        }

        public void DeleteTarget(int targetId)
        {
            Database.DeleteTarget(targetId);
        }

        public void FinalTarget(int targetId)
        {
            Database.FinalTarget(targetId);
        }

        public void CancelFinalTarget(int targetId)
        {
            Database.CancelFinalTarget(targetId);
        }

        public List<ExchangeOrderOffer> GetAvailableOrdersId(int targetId)
        {
            return Database.GetAvailableOrdersIdByTargetId(targetId);
        }

        public void UpdateTarget(Target target)
        {
            Database.UpdateTarget(target);
        }

        public ExchangeOrderOffer GetExchangeOfferById(int offerId)
        {
            return Database.GetOfferById(offerId);
        }

        public void ClearAll()
        {
            IsTargetOrder = false;
            IsSenderOrders = false;
            ClearLists();
            CurrentExchangeOrder = null;
            ValueItemType = 0;
            CurrentTarget = null;
        }

        public List<List<ExchangeOrder>> GetRecommendedOrdersBySearch(string search, List<List<ExchangeOrder>> orders)
        {
            var sortOrders = new List<List<ExchangeOrder>>();

            foreach (var orderList in orders)
            {      
                    if(Database.CheckNameRecommendedOrders(search, orderList))
                    {
                        sortOrders.Add(orderList);
                    }
            }

            return sortOrders;
        }

        public List<ExchangeOrder> GetFilterOrders(string email)
        {
            return Database.GetFilterOrders(email, FilterOrders);
        }

        public List<List<ExchangeOrder>> GetRecommendedOrdersByFilter(List<List<ExchangeOrder>> recOrders)
        {
            var returnOrders = new List<List<ExchangeOrder>>();
            bool isExistFilterOrders = false;

            foreach(var orderList in recOrders)
            {
                foreach(var filterOrder in FilterOrders)
                {
                    isExistFilterOrders = false;
                    foreach (var order in orderList)
                    {
                        if(filterOrder.ExchangeOrderId == order.ExchangeOrderId)
                        {
                            isExistFilterOrders=true;
                            break;
                        }
                    }

                    if(!isExistFilterOrders)
                    {
                        break;
                    }
                }

                if(isExistFilterOrders)
                {
                    returnOrders.Add(orderList);
                }
            }

            return returnOrders;
        }

        public List<ExchangeOrderOffer> SearchOffers(string searchText, List<ExchangeOrderOffer> offers)
        {
            var returnOffers = new List<ExchangeOrderOffer>();
            bool isContinue;

            foreach(var offer in offers)
            {
                isContinue = false;
                if (CheckOfferSearch(searchText, Database.GetOrderListByIdList(offer.SenderExchangeOrdersId)))
                {
                    returnOffers.Add(offer);
                    isContinue = true;
                }

                if(!isContinue && CheckOfferSearch(searchText, Database.GetOrderListByIdList(offer.RecipientExchangeOrdersId)))
                {
                    returnOffers.Add(offer);
                }
            }

            return returnOffers;
        }

        private bool CheckOfferSearch(string searchText, List<ExchangeOrder> orders)
        {
            var itemTypeList = Database.GetAllItemTypes();
            var itemCategoryList = Database.GetAllItemCategories();

            foreach (var item in orders)
            {
                if (item.Title.ToLower().Contains(searchText.ToLower()))
                {
                    return true;
                }

                foreach (var itemType in itemTypeList)
                {
                    if (itemType.Title.ToLower().Contains(searchText.ToLower()))
                    {
                        if (itemType.ItemTypeId == item.ItemTypeId)
                        {
                            return true;
                        }
                    }
                }

                foreach (var itemCategory in itemCategoryList)
                {
                    if (itemCategory.Title.ToLower().Contains(searchText.ToLower()))
                    {
                        var type = Database.GetItemTypeById(item.ItemTypeId);
                        if (type.ItemCategoryId == itemCategory.ItemCategoryId)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
