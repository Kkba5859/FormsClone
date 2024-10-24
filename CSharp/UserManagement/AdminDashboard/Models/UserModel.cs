using FormsClone.CSharp.UserManagement.AbstractClasses;

namespace FormsClone.CSharp.UserManagement.AdminDashboard.Models
{
    public class UserModel : BaseUserModel
    {
        public bool IsBlocked { get; set; }

        public UserModel() : base() { }

        public UserModel(string username, string password, bool isAdmin , bool isBlocked = false)
            : base(username, password, isAdmin)
        {
            IsBlocked = isBlocked;
        }
    }
}
