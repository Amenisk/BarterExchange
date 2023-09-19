using BarterExchange.Data.Classes;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace BarterExchange.Data.Services
{
    public class UserService
    {
        public User CurrentUser { get; private set; }  
        public bool CheckEmail(string email)
        {
            var user = Database.GetUserByEmail(email);

            return user != null;
        }

        public bool CheckPhoneNumber(string phoneNumber)
        {
            var user  = Database.GetUserByPhoneNumber(phoneNumber);

            return user != null;
        }

        public bool CheckCorrectEmail(string email)
        {
            string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
            Match isMatch = Regex.Match(email, pattern, RegexOptions.IgnoreCase);
            return !isMatch.Success;
        }

        public bool CheckCorrectPhoneNumber(string phoneNumber)
        {
            string pattern = "^((8|\\+7)[\\- ]?)?(\\(?\\d{3}\\)?[\\- ]?)?[\\d\\- ]{7,10}$";
            Match isMatch = Regex.Match(phoneNumber, pattern, RegexOptions.IgnoreCase);
            return !isMatch.Success;
        }

        public void RegisterUser(User user)
        {
            Database.SaveUser(user);
        }

        public bool AuthorizeUser(string emailOrPhoneNumber, string password)
        {
            var user1 = Database.AuthorizeUserByEmail(emailOrPhoneNumber, password);
            var user2 = Database.AuthorizeUserByPhoneNumber(emailOrPhoneNumber, password);

            if(user1 != null)
            {
                CurrentUser = user1;
            } 
            else if (user2 != null)
            {
                CurrentUser = user2;
            }
            else
            {
                return false;
            }

            return true;
        }

        public bool IsAuthorized()
        {
            return CurrentUser != null;
        }

        public void LogOut()
        {
            CurrentUser = null;
        }


    }
}
