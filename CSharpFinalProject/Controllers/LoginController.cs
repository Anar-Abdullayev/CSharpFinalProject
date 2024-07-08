using CSharpFinalProject.Models;

namespace CSharpFinalProject.Controllers
{
    internal static class LoginController
    {
        public static User? Login(string username, string password)
        {
            User? user = Database.Users.FirstOrDefault(x=>x.Username == username && x.Password == password);
            return user;
        }
    }
}
