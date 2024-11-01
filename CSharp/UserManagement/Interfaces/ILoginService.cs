using FormsClone.CSharp.UserManagement.AdminDashboard.Models; // Подключение модели UserModel.

namespace FormsClone.CSharp.UserManagement.Interfaces
{
    public interface ILoginService // Интерфейс ILoginService для управления процессом входа пользователей.
    {
        // Асинхронный метод для входа пользователя.
        Task<(bool Success, string Message, UserModel User)> LoginUserAsync(string? username, string? password);

        // Асинхронный метод для входа администратора.
        Task<bool> LoginAdminAsync(string username, string password);

        // Асинхронный метод для получения текущего пользователя.
        Task<UserModel> GetCurrentUserAsync();

        // Асинхронный метод для установки состояния пользователя как вошедшего в систему.
        Task SetLoggedInState(UserModel user);

        // Асинхронный метод для проверки, вошел ли пользователь в систему.
        Task<bool> IsUserLoggedInAsync();

        // Асинхронный метод для выхода пользователя из системы.
        Task LogoutUserAsync();

        // Асинхронный метод для проверки, является ли пользователь администратором.
        Task<bool> IsUserAdminAsync();
    }
}
