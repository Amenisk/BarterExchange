using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BarterExchange.Data.Classes
{
    public class User
    {
        [BsonIgnoreIfDefault]
        public ObjectId _id;
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Password { get; private set; }
        public string Role { get; private set; }
        public int VipLevel { get; private set; }
        public DateTime EndDateVipLevel { get; private set; }

        public User(string name, string surname, string email, 
            string phoneNumber, string password) 
        {
            Name = name;
            Surname = surname;
            Email = email;
            PhoneNumber = phoneNumber;
            Password = password;
            Role = "User";
            VipLevel = 0;
            EndDateVipLevel = DateTime.Now;
        }

        public void SetLevel(int level)
        {
            VipLevel = level;
        }

        public void SetEndDateVipLevel(DateTime date) 
        {
            EndDateVipLevel = date;
        }
    }
}
