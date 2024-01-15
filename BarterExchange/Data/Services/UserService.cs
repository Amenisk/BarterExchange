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
            emailOrPhoneNumber = UserService.EncodeDecrypt(emailOrPhoneNumber, Storage.Key);
            password = UserService.EncodeDecrypt(password, Storage.Key);

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

        public void EditUserProfile(User user)
        {
            Database.EditUser(user);
            CurrentUser = user;
        }

        public static string EncodeDecrypt(string str, ushort secretKey)
        {
            var ch = str.ToArray(); //преобразуем строку в символы
            string newStr = "";      //переменная которая будет содержать зашифрованную строку
            foreach (var c in ch)  //выбираем каждый элемент из массива символов нашей строки
                newStr += TopSecret(c, secretKey);  //производим шифрование каждого отдельного элемента и сохраняем его в строку
            return newStr;
        }

        private static char TopSecret(char character, ushort secretKey)
        {
            character = (char)(character ^ secretKey); //Производим XOR операцию
            return character;
        }


    }
}
