namespace FormsClone.CSharp.UserManagement.Interfaces
{
    public interface IUserModel
    {
        int Id { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        bool IsAdmin { get; set; }
    }
}
