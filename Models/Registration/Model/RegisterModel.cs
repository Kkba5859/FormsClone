using FormsClone.Models.User.Model;
using System.ComponentModel.DataAnnotations;

namespace FormsClone.Models.Registration.Model
{
    public class RegisterModel : IUserModel
    {
        public RegisterModel()
        {
            Username = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            IsAdmin = false;
        }

        public RegisterModel(string username, string email, string password, bool isAdmin)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            IsAdmin = isAdmin;
        }

        public int Id { get; set; } // Реализуем Id из IUserModel

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        public bool IsAdmin { get; set; } // Flag to indicate if registering as admin
    }
}