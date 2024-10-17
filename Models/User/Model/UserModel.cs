using System.ComponentModel.DataAnnotations;

namespace FormsClone.Models.User.Model
{
    public class UserModel : IUserModel
    {
        public UserModel()
        {
            Username = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            IsAdmin = false;
            IsBlocked = false;
        }

        public UserModel(string username, string email, string password, bool isAdmin = false, bool isBlocked = false)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            IsAdmin = isAdmin;
            IsBlocked = isBlocked;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, ErrorMessage = "Username cannot be longer than 30 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(30, ErrorMessage = "Email cannot be longer than 30 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(30, ErrorMessage = "Password cannot be longer than 30 characters.")]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsBlocked { get; set; }
    }
}