using FormsAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace FormsAPI.Services
{
    public class UserService : IUserService
    {
        List<User> users = new List<User>()
        {
            new User(){ Email = "isaacfallasv@gmail.com", Password = "ifv123"}
        };

        public bool IsUser(string email, string password) =>
            users.Where(d=>d.Email == email && d.Password == password).Count() > 0;
    }
}
