using FormsClone.CSharp.UserManagement.AbstractClasses;

namespace FormsClone.CSharp.UserManagement.Login.Models
{
    public class LoginModel : BaseUserModel
    {
        public LoginModel() : base() { }

        public LoginModel(string username, string password, bool isAdmin)
            : base(username, password, isAdmin)
        {
        }
    }
}
