using Blazored.LocalStorage;
using FormsClone.Models.Registration;
using FormsClone.Models.Registration.Model;
using FormsClone.Models.User.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FormsClone.Models.User.Service
{
    public class UserService : IUserService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _navigationManager;
        private readonly IJSRuntime _jsRuntime;
        private readonly AuthStateService _authStateService; // Добавляем AuthStateService

        public UserService(ILocalStorageService localStorage, NavigationManager navigationManager, IJSRuntime jsRuntime, AuthStateService authStateService)
        {
            _localStorage = localStorage;
            _navigationManager = navigationManager;
            _jsRuntime = jsRuntime;
            _authStateService = authStateService; // Присваиваем AuthStateService
        }

        // Регистрация
        public async Task<RegistrationResult> RegisterUserAsync(RegisterModel? model, bool isAdmin = false)
        {
            if (model == null)
                return new RegistrationResult(false, new List<string> { "Registration model cannot be null." });

            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                return new RegistrationResult(false, new List<string> { "Username, Email, and Password must not be empty." });

            try
            {
                var existingUser = await _localStorage.GetItemAsync<UserModel>($"user_{model.Username}");
                if (existingUser != null)
                    return new RegistrationResult(false, new List<string> { "User already exists." });

                var user = new UserModel(username: model.Username, email: model.Email, password: model.Password, isAdmin: isAdmin);
                await _localStorage.SetItemAsync($"user_{model.Username}", user);

                return new RegistrationResult(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при регистрации пользователя: {ex.Message}");
                return new RegistrationResult(false, new List<string> { "Ошибка при регистрации пользователя." });
            }
        }

        public async Task<RegistrationResult> RegisterAdminAsync(RegisterModel model)
        {
            return await RegisterUserAsync(model, true);
        }

        // Аутентификация
        public async Task<bool> LoginUserAsync(string? username, string? password)
        {
            try
            {
                var user = await _localStorage.GetItemAsync<UserModel>($"user_{username}");
                if (user != null && user.Password == password && !user.IsBlocked)
                {
                    await _localStorage.SetItemAsync("currentUser", user);
                    _authStateService.IsLoggedIn = true; // Устанавливаем состояние аутентификации
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при входе пользователя: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> LoginAdminAsync(string? username, string? password)
        {
            try
            {
                var user = await _localStorage.GetItemAsync<UserModel>($"user_{username}");
                if (user != null && user.Password == password && user.IsAdmin && !user.IsBlocked)
                {
                    await _localStorage.SetItemAsync("currentUser", user);
                    _authStateService.IsLoggedIn = true; // Устанавливаем состояние аутентификации
                    NavigateToDashboard();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при входе администратора: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> IsUserLoggedInAsync()
        {
            try
            {
                var user = await _localStorage.GetItemAsync<UserModel>("currentUser");
                return user != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при проверке состояния пользователя: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> IsUserAdminAsync()
        {
            try
            {
                var user = await _localStorage.GetItemAsync<UserModel>("currentUser");
                return user?.IsAdmin ?? false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при проверке прав администратора: {ex.Message}");
                return false;
            }
        }

        public async Task SetLoggedInState(UserModel user)
        {
            try
            {
                await _localStorage.SetItemAsync("currentUser", user);
                _authStateService.IsLoggedIn = true; // Устанавливаем состояние аутентификации
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при установке состояния пользователя: {ex.Message}");
            }
        }

        public async Task LogoutUserAsync()
        {
            try
            {
                await _localStorage.RemoveItemAsync("currentUser");
                _authStateService.IsLoggedIn = false; // Обновляем состояние аутентификации
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выходе пользователя: {ex.Message}");
            }
        }

        public async Task<UserModel> GetCurrentUserAsync()
        {
            try
            {
                return await _localStorage.GetItemAsync<UserModel>("currentUser") ?? throw new InvalidOperationException();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении текущего пользователя: {ex.Message}");
                throw;
            }
        }

        // Управление пользователями
        public async Task<List<UserModel>> GetUsersAsync()
        {
            var users = new List<UserModel>();
            try
            {
                foreach (var key in await _localStorage.KeysAsync())
                {
                    if (key == "currentUser" || !key.StartsWith("user_")) continue;
                    var user = await _localStorage.GetItemAsync<UserModel>(key);
                    if (user != null && !users.Exists(u => u.Username == user.Username))
                    {
                        users.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении списка пользователей: {ex.Message}");
            }
            return users;
        }

        public async Task BlockUserAsync(string userId)
        {
            await UpdateUserStatusAsync(userId, true, isBlocked: true);
        }

        public async Task UnblockUserAsync(string userId)
        {
            await UpdateUserStatusAsync(userId, false, isBlocked: false);
        }

        public async Task DeleteUserAsync(string userId)
        {
            try
            {
                await _localStorage.RemoveItemAsync($"user_{userId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении пользователя: {ex.Message}");
            }
        }

        public async Task AddToAdminAsync(string userId)
        {
            await UpdateUserStatusAsync(userId, true, isAdmin: true);
            try
            {
                var user = await _localStorage.GetItemAsync<UserModel>($"user_{userId}");
                if (user != null)
                {
                    user.IsAdmin = true;
                    await _localStorage.SetItemAsync($"user_{userId}", user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении пользователя в администраторы: {ex.Message}");
            }
        }

        public async Task RemoveFromAdminAsync(string userId)
        {
            await UpdateUserStatusAsync(userId, false, isAdmin: false);
            try
            {
                var user = await _localStorage.GetItemAsync<UserModel>($"user_{userId}");
                if (user != null)
                {
                    user.IsAdmin = false;
                    await _localStorage.SetItemAsync($"user_{userId}", user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении пользователя из администраторов: {ex.Message}");
            }
        }

        private async Task UpdateUserStatusAsync(string userId, bool value, bool isBlocked = false, bool isAdmin = false)
        {
            try
            {
                var user = await _localStorage.GetItemAsync<UserModel>($"user_{userId}");
                if (user != null)
                {
                    if (isBlocked)
                        user.IsBlocked = value;
                    if (isAdmin)
                        user.IsAdmin = value;

                    await _localStorage.SetItemAsync($"user_{userId}", user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении статуса пользователя: {ex.Message}");
            }
        }

        public async Task<string> GetUserNameAsync()
        {
            try
            {
                var user = await _localStorage.GetItemAsync<UserModel>("currentUser");
                return user?.Username ?? string.Empty; // Вернуть имя пользователя или пустую строку
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении имени пользователя: {ex.Message}");
                return string.Empty; // Вернуть пустую строку в случае ошибки
            }
        }

        public void NavigateToDashboard()
        {
            _navigationManager.NavigateTo("/dashboard");
        }
    }

    public class AuthStateService
    {
        public event Action? OnChange;

        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                _isLoggedIn = value;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
