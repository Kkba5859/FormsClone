using FormsClone.CSharp.UserManagement.AdminDashboard.Models;

namespace FormsClone.CSharp.UserManagement.Interfaces
{
    public interface IUserService
    {
        Task<List<UserModel>> GetUsersAsync(); 
        Task BlockUserAsync(string userId); 
        Task UnblockUserAsync(string userId); 
        Task DeleteUserAsync(string userId); 
        Task AddToAdminAsync(string userId);
        Task RemoveFromAdminAsync(string userId); 
        Task<string> GetUserNameAsync(); 
    }
}