using MongoDB.Driver;

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
    }
}
