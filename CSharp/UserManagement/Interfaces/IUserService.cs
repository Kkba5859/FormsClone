using FormsClone.CSharp.UserManagement.AdminDashboard.Models; // Подключение модели UserModel.

namespace FormsClone.CSharp.UserManagement.Interfaces
{
    public interface IUserService // Интерфейс IUserService для управления пользователями.
    {
        // Асинхронный метод для получения списка пользователей.
        Task<List<UserModel>> GetUsersAsync();

        // Асинхронный метод для блокировки пользователя по его идентификатору.
        Task BlockUserAsync(string userId);

        // Асинхронный метод для разблокировки пользователя по его идентификатору.
        Task UnblockUserAsync(string userId);

        // Асинхронный метод для удаления пользователя по его идентификатору.
        Task DeleteUserAsync(string userId);

        // Асинхронный метод для добавления пользователя в администраторы.
        Task AddToAdminAsync(string userId);

        // Асинхронный метод для удаления пользователя из администраторов.
        Task RemoveFromAdminAsync(string userId);

        // Асинхронный метод для получения имени текущего пользователя.
        Task<string> GetUserNameAsync();
        Task<bool> ChangePasswordAsync(string userId, string newPassword);
    }
}
