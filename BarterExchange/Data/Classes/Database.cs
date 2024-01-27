using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.Collections;
using System.Collections.Generic;
using static MudBlazor.CategoryTypes;

namespace BarterExchange.Data.Classes
{  
    public static class Database
    {
        private const double VALUE_PROCENT = 0.1;
        private const int MAX_LEVEL = 3;
        static MongoClient client = new MongoClient("mongodb://localhost");
        static IMongoDatabase database = client.GetDatabase("BarterExchangeService");
        public static void SaveUser(User user)
        {
            var collection = database.GetCollection<User>("Users");

            collection.InsertOne(user);
        }

        public static User GetUserByEmail(string email)
        {
            var collection = database.GetCollection<User>("Users");

            return collection.Find(x => x.Email == email).FirstOrDefault();
        }
        public static User GetUserByPhoneNumber(string phoneNumber)
        {
            var collection = database.GetCollection<User>("Users");

            return collection.Find(x => x.PhoneNumber == phoneNumber).FirstOrDefault();
        }

        public static User AuthorizeUserByEmail(string email, string password)
        {
            var collection = database.GetCollection<User>("Users");

            return collection.Find(x => x.Email == email && x.Password == password).FirstOrDefault();
        }
        public static User AuthorizeUserByPhoneNumber(string phoneNumber, string password)
        {
            var collection = database.GetCollection<User>("Users");

            return collection.Find(x => x.PhoneNumber == phoneNumber && x.Password == password).FirstOrDefault();
        }

        public static async Task UploadImageToDbAsync(Stream fs, string name)
        {
            var gridFS = new GridFSBucket(database);

            await gridFS.UploadFromStreamAsync(name, fs);
        }

        public static void DownloadToLocal(string name)
        {
            var gridFS = new GridFSBucket(database);
            using (FileStream fs = new FileStream($"{Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/wwwroot/Images/")}{name}", FileMode.OpenOrCreate))
            {
                gridFS.DownloadToStreamByName(name, fs);
            }
        }

        public static ExchangeOrder GetLastExchangeOrder()
        {
            var collection = database.GetCollection<ExchangeOrder>("ExchangeOrders");
            var list = collection.Find(x => x.Title != null).ToList();
            list.Reverse();

            return list.FirstOrDefault();
        }

        public static void SaveExchangeOrder(ExchangeOrder order)
        {
            var collection = database.GetCollection<ExchangeOrder>("ExchangeOrders");

            collection.InsertOne(order);
        }

        public static ItemType GetItemTypeByTitle(string title)
        {
            var collection = database.GetCollection<ItemType>("ItemTypes");

            return collection.Find(x => x.Title == title).FirstOrDefault();
        }

        public static void SaveItemCategory(ItemCategory category)
        {
            var collection = database.GetCollection<ItemCategory>("ItemCategories");

            collection.InsertOne(category);
        }

        public static ItemCategory GetLastItemCategory()
        {
            var collection = database.GetCollection<ItemCategory>("ItemCategories");
            var list = collection.Find(x => x.Title != null).ToList();
            list.Reverse();

            return list.FirstOrDefault();
        }

        public static ItemType GetLastItemType()
        {
            var collection = database.GetCollection<ItemType>("ItemTypes");
            var list = collection.Find(x => x.Title != null).ToList();
            list.Reverse();

            return list.FirstOrDefault();
        }

        public static ItemCategory GetItemCategoryByTitle(string title)
        {
            var collection = database.GetCollection<ItemCategory>("ItemCategories");

            return collection.Find(x => x.Title == title).FirstOrDefault();
        }

        public static bool CheckPresenceItemTypeByItemCategoryId(int itemCategoryId)
        {
            var collection = database.GetCollection<ItemType>("ItemTypes");

            return collection.Find(x => x.ItemCategoryId == itemCategoryId).FirstOrDefault() != null;
        }

        public static List<ItemCategory> GetAllItemCategories()
        {
            var collection = database.GetCollection<ItemCategory>("ItemCategories");

            return collection.Find(x => x.Title != null).ToList();
        }
        public static List<ItemType> GetAllItemTypes()
        {
            var collection = database.GetCollection<ItemType>("ItemTypes");

            return collection.Find(x => x.Title != null).ToList();
        }

        public static List <ItemType> GetItemTypesByCategory(int itemCategoryId) 
        {
            var collection = database.GetCollection<ItemType>("ItemTypes");

            return collection.Find(x => x.ItemCategoryId == itemCategoryId).ToList();
        }

        public static void DeleteItemCategoryByTitle(string title)
        {
            var collection = database.GetCollection<ItemCategory>("ItemCategories");

            collection.DeleteOne(x => x.Title == title);
        }

        public static void SaveItemType(ItemType type)
        {
            var collection = database.GetCollection<ItemType>("ItemTypes");

            collection.InsertOne(type);
        }

        public static void EditValueItemType(string title, int value)
        {
            var collection = database.GetCollection<ItemType>("ItemTypes");
            var filter = Builders<ItemType>.Filter.Eq("Title", title);
            var update = Builders<ItemType>.Update.Set("Value", value);

            collection.UpdateOne(filter, update);
        }
        public static void DeleteItemTypeByTitle(string title)
        {
            var collection = database.GetCollection<ItemType>("ItemTypes");

            collection.DeleteOne(x => x.Title == title);
        }

        public static List<ExchangeOrder> GetAllExchangeOrders()
        {
            var collection = database.GetCollection<ExchangeOrder>("ExchangeOrders");
            var list = new List<ExchangeOrder>();

            for(int i = MAX_LEVEL; i >= 0; i--)
            {
                foreach (var order in collection.Find(x => x.IsConducted == false).ToList())
                {
                    var user = GetUserByEmail(order.CreatorEmail);

                    if(user.VipLevel == i)
                    {
                        list.Add(order);
                    }
                }
            }

            return list;
        }

        public static ExchangeOrder GetExchangeOrderById(int id)
        {
            var collection = database.GetCollection<ExchangeOrder>("ExchangeOrders");

            return collection.Find(x => x.ExchangeOrderId == id).FirstOrDefault();
        }

        public static ItemType GetItemTypeById(int id) 
        {
            var collection = database.GetCollection<ItemType>("ItemTypes");

            return collection.Find(x => x.ItemTypeId == id).FirstOrDefault();
        }

        public static ItemCategory GetItemCategoryById(int id)
        {
            var collection = database.GetCollection<ItemCategory>("ItemCategories");

            return collection.Find(x => x.ItemCategoryId == id).FirstOrDefault();
        }

        public static List<ExchangeOrder> GetExchangeOrdersByCreatorEmailAndConduct(string creatorEmail, bool isConducted)
        {
            var collection = database.GetCollection<ExchangeOrder>("ExchangeOrders");

            return collection.Find(x =>x.CreatorEmail == creatorEmail && x.IsConducted == isConducted).ToList();
        }

        public static void ConductExchangeOrder(int id)
        {
            var collection = database.GetCollection<ExchangeOrder>("ExchangeOrders");
            var filter = Builders<ExchangeOrder>.Filter.Eq("ExchangeOrderId", id);
            var update = Builders<ExchangeOrder>.Update.Set("IsConducted", true);

            collection.UpdateOne(filter, update);
        }

        public static void DeletePhoto(string photoName)
        {
            var gridFS = new GridFSBucket(database);
            var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, photoName);
            var fileInfo = gridFS.Find(filter).FirstOrDefault();

            gridFS.Delete(fileInfo.Id);
        }

        public static void DeleteExchangeOrder(int id)
        {
            var collection = database.GetCollection<ExchangeOrder>("ExchangeOrders");
            var order = collection.Find(x => x.ExchangeOrderId == id).FirstOrDefault();
            DeletePhoto(order.PhotoName);

            collection.DeleteOne(x => x.ExchangeOrderId == id);           
        }

        public static void ReplaceExchangeOrder(ExchangeOrder order)
        {
            var collection = database.GetCollection<ExchangeOrder>("ExchangeOrders");

            collection.ReplaceOne(x => x.ExchangeOrderId == order.ExchangeOrderId, order);
        }

        public static void SaveExchangeOrderOffer(ExchangeOrderOffer offer)
        {
            var collection = database.GetCollection<ExchangeOrderOffer>("ExchangeOrderOffers");

            collection.InsertOne(offer);
        }

        public static ExchangeOrderOffer GetExchangeOrderOfferById(int offerId)
        {
            var collection = database.GetCollection<ExchangeOrderOffer>("ExchangeOrderOffers");

            return collection.Find(x => x.ExchangeOfferId == offerId).FirstOrDefault();
        }

        public static List<ExchangeOrderOffer> GetExchangeOffersBySenderEmail(string senderEmail)
        {
            var collection = database.GetCollection<ExchangeOrderOffer>("ExchangeOrderOffers");

            return collection.Find(x => x.SenderEmail == senderEmail && x.IsConducted == false).ToList();
        }

        public static List<ExchangeOrderOffer> GetExchangeOffersByRecipientEmail(string recipientEmail)
        {
            var collection = database.GetCollection<ExchangeOrderOffer>("ExchangeOrderOffers");

            return collection.Find(x => x.RecipientEmail == recipientEmail && x.IsConducted == false).ToList();
        }

        public static void AcceptExchangeOffer(ExchangeOrderOffer offer)
        {
            var collection = database.GetCollection<ExchangeOrderOffer>("ExchangeOrderOffers");
            var filter = Builders<ExchangeOrderOffer>.Filter.Eq("ExchangeOfferId", offer.ExchangeOfferId);
            var update = Builders<ExchangeOrderOffer>.Update.Set("IsConducted", true);

            foreach(var s in offer.SenderExchangeOrdersId)
            {
                ConductExchangeOrder(s);
            }
            foreach(var r in offer.RecipientExchangeOrdersId)
            {
                ConductExchangeOrder(r);
            }

            collection.UpdateOne(filter, update);
        }

        public static List<ExchangeOrderOffer> GetAllConductedOffersByUserEmail(string email)
        {
            var collection = database.GetCollection<ExchangeOrderOffer>("ExchangeOrderOffers");

            return collection.Find(x => (x.SenderEmail == email || x.RecipientEmail == email) && x.IsConducted == true).ToList();
        }

        public static List<ExchangeOrder> SearchByTitleCategoryAndTypeItem(string searchText)
        {
            var collection = database.GetCollection<ExchangeOrder>("ExchangeOrders");
            var listOrders = new List<ExchangeOrder>();
            var itemTypesList = GetAllItemTypes();
            var itemCategoriesList = GetAllItemCategories();
            bool isContinue;


            foreach (var item in collection.Find(x => !x.IsConducted).ToList())
            {
                isContinue = false;

                if (item.Title.ToLower().Contains(searchText.ToLower()))
                {
                    listOrders.Add(item);
                    continue;
                }

                foreach(var itemType in itemTypesList)
                {
                    if(itemType.Title.ToLower().Contains(searchText.ToLower()))
                    {
                        if(itemType.ItemTypeId == item.ItemTypeId)
                        {
                            listOrders.Add(item);
                            isContinue = true;
                        }
                    }
                }

                if(isContinue)
                {
                    continue;
                }

                foreach (var itemCategory in itemCategoriesList)
                {
                    if(itemCategory.Title.ToLower().Contains(searchText.ToLower()))
                    {
                        var type = GetItemTypeById(item.ItemTypeId);
                        if(type.ItemCategoryId == itemCategory.ItemCategoryId)
                        {
                            listOrders.Add(item);
                        }
                    }
                }
            }

            return listOrders;
        }

        public static List<ExchangeOrder> GetRecomendedOrdersByUserEmail(string email)
        {
            var collection = database.GetCollection<ExchangeOrder>("ExchangeOrders");
            var orders = collection.Find(x => x.IsConducted == false && x.CreatorEmail != email).ToList();
            var userOrders = collection.Find(x => x.IsConducted == false && x.CreatorEmail == email).ToList();
            var ordersList = new List<ExchangeOrder>(); 

            foreach(var order in orders)
            {
                var type = GetItemTypeById(order.ItemTypeId);

                    if (GetRelevantExchangeOrdersByRecommendation(email, type.Value).Count() != 0)
                    {
                        if(!ordersList.Contains(order))
                        {
                            ordersList.Add(order);
                        }      
                        continue;
                    }
            }

            return ordersList;
        }

        public static bool CheckExchangeOrdersByUserEmail(string email)
        {
            var collection = database.GetCollection<ExchangeOrder>("ExchangeOrders");

            return collection.Find(x => x.CreatorEmail == email).FirstOrDefault() != null;
        }

        public static List<List<ExchangeOrder>> GetRelevantExchangeOrdersByRecommendation(string email, int value)
        {
            var collection = database.GetCollection<ExchangeOrder>("ExchangeOrders");
            var ordersList = new List<List<int>>();
            var ordersId = new List<int>();
            var readyOrders = new List<List<ExchangeOrder>>();
            var orders = collection.Find(x => x.CreatorEmail == email && x.IsConducted == false).ToList();

            foreach ( var order in orders)
            {
                ordersId.Add(order.ExchangeOrderId);
            }

            for(var i = 1; i <= orders.Count(); i++ )
            {
                var result = GetKCombs(ordersId, i);
                var resultList = new List<List<int>>();

                foreach(var r in result)
                {
                    resultList.Add(r.ToList());
                }
                ordersList.AddRange(resultList);
            }

            foreach (var o in ordersList)
            {
                var val = 0;
                var list = new List<ExchangeOrder>();
                foreach(var id in o)
                {
                    var ord = GetExchangeOrderById(id);
                    list.Add(ord);
                    var itemType = GetItemTypeById(ord.ItemTypeId);
                    val += itemType.Value;
                }

                if (val * (1 - VALUE_PROCENT) <= value && val * (1 + VALUE_PROCENT) >= value)
                {
                    readyOrders.Add(list);
                }
            }

            return readyOrders;
        }

        static IEnumerable<IEnumerable<T>>GetKCombs<T>(IEnumerable<T> list, int length) where T : IComparable
        {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetKCombs(list, length - 1)
                .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }
        public static void EditUser(User user)
        {
            var collection = database.GetCollection<User>("Users");

            collection.ReplaceOne(x => x.Email == user.Email, user);
        }

        public static void RejectExchangeOffer(int offerId)
        {
            var collection = database.GetCollection<ExchangeOrderOffer>("ExchangeOrderOffers");

            collection.DeleteOne(x => x.ExchangeOfferId == offerId);
        }

        public static bool CheckItemTypeAvailabilityOfOrder(int itemTypeId)
        {
            var collection = database.GetCollection<ExchangeOrder>("ExchangeOrders");

            return collection.Find(x => x.ItemTypeId == itemTypeId).FirstOrDefault() != null;
        }

        public static int GetLastExchangeOfferId()
        {
            var collection = database.GetCollection<ExchangeOrderOffer>("ExchangeOrderOffers");

            var offer = collection.Find(x => x.ExchangeOfferId != 0).FirstOrDefault();

            if(offer != null)
            {
                return offer.ExchangeOfferId;
            }
            else
            {
                return 0;
            }
        }

        public static bool CheckExchangeOrderOffer(ExchangeOrderOffer offer)
        {
            var collection = database.GetCollection<ExchangeOrderOffer>("ExchangeOrderOffers");

            var list = collection.Find(x => x.ExchangeOfferId != 0).ToList();

            foreach(var of in list)
            {
                if(new HashSet<int>(of.SenderExchangeOrdersId).SetEquals(offer.SenderExchangeOrdersId) 
                    && new HashSet<int>(of.RecipientExchangeOrdersId).SetEquals(offer.RecipientExchangeOrdersId))
                {
                    return true;
                }


                if (new HashSet<int>(of.SenderExchangeOrdersId).SetEquals(offer.RecipientExchangeOrdersId)
                    && new HashSet<int>(of.RecipientExchangeOrdersId).SetEquals(offer.SenderExchangeOrdersId))
                {
                    return true;
                }
            }

            return false;
        }

        public static void BuyVipLevel(string email, int level, int countDays)
        {
            var collection = database.GetCollection<User>("Users");
            var user = collection.Find(x => x.Email == email).FirstOrDefault();

            user.SetLevel(level);
            if(user.EndDateVipLevel.Date < DateTime.Now.Date)
            {
                user.SetEndDateVipLevel(DateTime.Now.AddDays(countDays));
            }
            else
            {
                user.SetEndDateVipLevel(user.EndDateVipLevel.AddDays(countDays));
            }
           
            collection.ReplaceOne(x => x.Email == user.Email, user);
        }

        public static void ReloadVipLevel()
        {
            var collection = database.GetCollection<User>("Users");

            
            var update = Builders<User>.Update.Set("VipLevel", 0);           
        

            foreach(var user in collection.Find(x => x.Email != null).ToList())
            {
                var filter = Builders<User>.Filter.Eq("Email", user.Email);

                if(user.EndDateVipLevel.Date < DateTime.Now.Date)
                {
                    collection.UpdateOne(filter, update);
                }                
            }
        }
    } 
}
