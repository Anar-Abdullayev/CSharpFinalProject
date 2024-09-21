using CSharpFinalProject.Models;
using System.Configuration;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace CSharpFinalProject.Controllers
{
    internal static class LoginController
    {
        public static User? Login(string username, string password)
        {
            User? user = Database.Users.FirstOrDefault(x=>x.Username == username && x.Password == password);
            return user;
        }

        public static bool Register(User user, out string registerResultMessage)
        {
            registerResultMessage = string.Empty;
            if (Database.Users.FirstOrDefault(x => x.Username == user.Username) is not null)
                throw new DuplicateNameException("The username already exists");
            if (string.IsNullOrEmpty(user.Username.Trim()) || string.IsNullOrEmpty(user.Password.Trim()))
                throw new ArgumentNullException("Username or password can not be empty");

            if (!PasswordValidation(user.Password))
            {
                registerResultMessage = "Password must contain at least one upper case, one numeric, and the length must be higher than 8 symbols";
                return false;
            }

            user.Username = user.Username.Trim();
            user.Password = user.Password.Trim();
            user.Name = user.Name.Trim();
            user.Surname = user.Surname.Trim();
            user.IsAdmin = false;

            Database.Users.Add(user);
            Database.Context.Users.Add(user);
            Database.Context.SaveChanges();
            registerResultMessage = "Registered succesfully!";
            return true;
        }

        public static bool PasswordValidation(string password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperCase = new Regex(@"[A-Z]+");
            var hasMinimumCharacters = new Regex(@".{8,}");

            if (!(hasNumber.IsMatch(password) && hasUpperCase.IsMatch(password) && hasMinimumCharacters.IsMatch(password)))
            {
                return false;
            }
            return true;
        }
    }
}
