namespace FormsClone.Models.User.Model
{
    public interface IUserModel
    {
        int Id { get; set; }
        string Username { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        bool IsAdmin { get; set; }
    }
}
