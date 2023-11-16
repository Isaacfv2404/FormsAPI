namespace FormsAPI.Services
{
    public interface IUserService
    {
        public bool IsUser(string email, string pass);
    }
}
