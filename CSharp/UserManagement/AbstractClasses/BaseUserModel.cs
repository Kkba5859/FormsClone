using FormsClone.CSharp.UserManagement.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace FormsClone.CSharp.UserManagement.AbstractClasses
{
    public abstract class BaseUserModel : IUserModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, ErrorMessage = "Username cannot be longer than 30 characters.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(30, ErrorMessage = "Password cannot be longer than 30 characters.")]
        public string Password { get; set; } = string.Empty;

        public bool IsAdmin { get; set; }

        protected BaseUserModel() { }

        protected BaseUserModel(string username, string password, bool isAdmin)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            IsAdmin = isAdmin;
        }
    }
}
