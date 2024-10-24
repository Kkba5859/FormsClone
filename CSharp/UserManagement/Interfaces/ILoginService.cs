using FormsClone.CSharp.UserManagement.AdminDashboard.Models;

namespace FormsClone.CSharp.UserManagement.Interfaces
{
    public interface ILoginService
    {
        Task<(bool Success, string Message, UserModel User)> LoginUserAsync(string? username, string? password);
        Task<bool> LoginAdminAsync(string username, string password);
        Task<UserModel> GetCurrentUserAsync();
        Task SetLoggedInState(UserModel user);
        Task<bool> IsUserLoggedInAsync(); 
        Task LogoutUserAsync(); 
        Task<bool> IsUserAdminAsync();
    }

}
