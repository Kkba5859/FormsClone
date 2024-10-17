using FormsClone.Models.Registration;
using FormsClone.Models.Registration.Model;
using FormsClone.Models.User.Model;

namespace FormsClone.Models.User.Service
{
    public interface IUserService
    {
        // Регистрация
        Task<RegistrationResult> RegisterUserAsync(RegisterModel model, bool isAdmin = false); // Регистрация пользователя
        Task<RegistrationResult> RegisterAdminAsync(RegisterModel model); // Регистрация администратора

        // Аутентификация
        Task<bool> LoginUserAsync(string? username, string? password); // Логин обычного пользователя
        Task<bool> LoginAdminAsync(string? username, string? password); // Логин администратора
        Task<bool> IsUserLoggedInAsync(); // Проверка, залогинен ли пользователь
        Task LogoutUserAsync(); // Логика выхода
        Task<bool> IsUserAdminAsync();
        Task SetLoggedInState(UserModel user); // Устанавливает состояние залогиненного пользователя
        Task<UserModel> GetCurrentUserAsync(); // Получение текущего пользователя

        // Управление пользователями
        Task<List<UserModel>> GetUsersAsync(); // Получение всех пользователей
        Task BlockUserAsync(string userId); // Блокировка пользователя
        Task UnblockUserAsync(string userId); // Разблокировка пользователя
        Task DeleteUserAsync(string userId); // Удаление пользователя
        Task AddToAdminAsync(string userId); // Добавление пользователя в администраторы
        Task RemoveFromAdminAsync(string userId); // Удаление пользователя из администраторов
        Task<string> GetUserNameAsync(); // Метод для получения имени текущего пользователя
    }
}