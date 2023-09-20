using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace BarterExchange.Data.Classes
{
    public static class Database
    {
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

        public static string DownloadToLocal(string name)
        {
            var gridFS = new GridFSBucket(database);
            using (FileStream fs = new FileStream($"{Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/wwwroot/Images/")}{name}", FileMode.OpenOrCreate))
            {
                gridFS.DownloadToStreamByName(name, fs);
            }

            return $"Images/{name}";
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
    }
}
